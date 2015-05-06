﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using NSoup.Nodes;
using NSoup.Parse;
using System.Globalization;

namespace NSoup.Helper
{
    /// <summary>
    /// Internal static utilities for handling data.
    /// </summary>
    public class DataUtil
    {
        private static readonly Regex _charsetPattern = new Regex("(?i)\\bcharset=\\s*\"?([^\\s;\"]*)", RegexOptions.Compiled);
        static readonly Encoding _defaultEncoding = Encoding.UTF8; // used if not found in header or meta charset
        //private static readonly int _bufferSize = 0x20000; // ~130K.

        private DataUtil() {}

        public static Encoding DefaultEncoding
        {
            get { return _defaultEncoding; }
        }
        
        /// <summary>
        /// Loads a file to a string.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="charsetName"></param>
        /// <returns></returns>
        public static Document Load(Stream input, string charsetName, string baseUri)
        {
            byte[] data = ReadToByteBuffer(input);

            Document doc = ParseByteData(data, charsetName, baseUri, Parser.HtmlParser());

            input.Close();

            return doc;
        }

        /// <summary>
        /// Parses a Document from an input steam, using the provided Parser.
        /// </summary>
        /// <param name="input">Input stream to parse. You will need to close it</param>
        /// <param name="charsetName">Character set of input</param>
        /// <param name="baseUri">Base URI of document, to resolve relative links against</param>
        /// <param name="parser">Alternate parser to use</param>
        /// <returns></returns>
        public static Document Load(Stream input, string charsetName, string baseUri, Parser parser) {
            byte[] data = ReadToByteBuffer(input);

            Document doc = ParseByteData(data, charsetName, baseUri, parser);

            input.Close();

            return doc;
        }

        // reads bytes first into a buffer, then decodes with the appropriate charset. done this way to support
        // switching the chartset midstream when a meta http-equiv tag defines the charset.
        public static Document ParseByteData(byte[] data, string charsetName, string baseUri, Parser parser)
        {
            string docData;
            Document doc = null;

            if (charsetName == null)
            {
                // determine from meta. safe parse as UTF-8

                // look for <meta http-equiv="Content-Type" content="text/html;charset=gb2312"> or HTML5 <meta charset="gb2312">
                docData = _defaultEncoding.GetString(data);
                doc = parser.ParseInput(docData, baseUri);
                Element meta = doc.Select("meta[http-equiv=content-type], meta[charset]").FirstOrDefault();

                if (meta != null)
                {
                    // if not found, will keep utf-8 as best attempt
                    string foundCharset = meta.HasAttr("http-equiv") ? GetCharsetFromContentType(meta.Attr("content")) : meta.Attr("charset");

                    if (foundCharset != null && foundCharset.Length != 0 && !foundCharset.Equals(_defaultEncoding.WebName.ToUpperInvariant()))
                    { // need to re-decode
                        charsetName = foundCharset;

                        docData = Encoding.GetEncoding(foundCharset).GetString(data);
                        doc = null;
                    }
                }
            }
            else
            {
                // specified by content type header (or by user on file load)
                if (string.IsNullOrEmpty(charsetName))
                {
                    throw new Exception("Must set charset arg to character set of file to parse. Set to null to attempt to detect from HTML");
                }

                docData = Encoding.GetEncoding(charsetName).GetString(data);
            }

            if (doc == null)
            {
                // there are times where there is a spurious byte-order-mark at the start of the text. Shouldn't be present
                // in utf-8. If after decoding, there is a BOM, strip it; otherwise will cause the parser to go straight
                // into head mode
                if (docData.Length > 0 && docData[0] == 65279)
                {
                    docData = docData.Substring(1);
                }

                doc = parser.ParseInput(docData, baseUri);
                doc.OutputSettings().SetEncoding(charsetName);
            }
            return doc;
        }

        public static byte[] ReadToByteBuffer(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[32768];

                int count = input.Read(buffer, 0, buffer.Length);
                ms.Write(buffer, 0, count);

                while (count > 0)
                {
                    count = input.Read(buffer, 0, buffer.Length);
                    ms.Write(buffer, 0, count);
                }
                
                return ms.ToArray();
            }
        }


        /// <summary>
        /// Parse out a charset from a content type header.  If the charset is not supported, returns null (so the default
        /// will kick in.)
        /// </summary>
        /// <param name="contentType">e.g. "text/html; charset=EUC-JP"</param>
        /// <returns>"EUC-JP", or null if not found. Charset is trimmed and uppercased.</returns>
        internal static string GetCharsetFromContentType(string contentType)
        {
            if (contentType == null)
            {
                return null;
            }

            Match m = _charsetPattern.Match(contentType);
            if (m.Success)
            {
                string charset = m.Groups[1].Value.Trim();


                try
                {
                    Encoding.GetEncoding(charset);
                    return charset;
                }
                catch { }

                charset = charset.ToUpper(CultureInfo.CreateSpecificCulture("en-US"));
                try
                {
                    Encoding.GetEncoding(charset);
                    return charset;
                }
                catch { }
            }
            return null;
        }
    }
}