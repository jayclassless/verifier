// $Id$

#region License
/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Verifier.
 *
 * The Initial Developer of the Original Code is Classless.net.
 * Portions created by the Initial Developer are Copyright (C) 2004 the Initial
 * Developer. All Rights Reserved.
 *
 * Contributor(s):
 *		Jason Simeone (jay@classless.net)
 * 
 * ***** END LICENSE BLOCK ***** */
#endregion

using System;
using System.Xml.Serialization;

namespace Classless.Verifier {
	/// <summary>The algorithm types supported by VerifyXML.</summary>
	public enum AlgorithmType {
		/// <summary>Adler32</summary>
		ADLER32,

		/// <summary>Cksum</summary>
		CKSUM,

		/// <summary>8bit CRC</summary>
		CRC8,

		/// <summary>8bit CRC (reversed polynomial)</summary>
		[XmlEnumAttribute("CRC8-REVERSED")]
		CRC8REVERSED,

		/// <summary>16bit CRC</summary>
		CRC16,

		/// <summary>16bit CRC (reversed polynomial)</summary>
		[XmlEnumAttribute("CRC16-REVERSED")]
		CRC16REVERSED,

		/// <summary>16bit CRC (CCITT standard)</summary>
		[XmlEnumAttribute("CRC16-CCITT")]
		CRC16CCITT,

		/// <summary>16bit CRC (CCITT standard, reversed polynomial)</summary>
		[XmlEnumAttribute("CRC16-CCITT-REVERSED")]
		CRC16CCITTREVERSED,

		/// <summary>16bit CRC (ARC)</summary>
		[XmlEnumAttribute("CRC16-ARC")]
		CRC16ARC,

		/// <summary>16bit CRC (ZMODEM)</summary>
		[XmlEnumAttribute("CRC16-ZMODEM")]
		CRC16ZMODEM,

		/// <summary>32bit CRC</summary>
		CRC32,

		/// <summary>32bit CRC (reversed polynomial)</summary>
		[XmlEnumAttribute("CRC32-REVERSED")]
		CRC32REVERSED,

		/// <summary>32bit CRC (BZip2)</summary>
		[XmlEnumAttribute("CRC32-BZIP2")]
		CRC32BZIP2,

		/// <summary>32bit CRC (JAMCRC)</summary>
		[XmlEnumAttribute("CRC32-JAMCRC")]
		CRC32JAMCRC,

		/// <summary>ELF Hash</summary>
		ELFHASH,

		/// <summary>16bit Frame Check Sequence</summary>
		FCS16,

		/// <summary>32bit Frame Check Sequence</summary>
		FCS32,

		/// <summary>32bit Fowler/Noll/Vo (FNV0)</summary>
		[XmlEnumAttribute("FNV0-32")]
		FNV032,

		/// <summary>64bit Fowler/Noll/Vo (FNV0)</summary>
		[XmlEnumAttribute("FNV0-64")]
		FNV064,

		/// <summary>32bit Fowler/Noll/Vo (FNV1)</summary>
		[XmlEnumAttribute("FNV1-32")]
		FNV132,

		/// <summary>64bit Fowler/Noll/Vo (FNV1)</summary>
		[XmlEnumAttribute("FNV1-64")]
		FNV164,

		/// <summary>32bit Fowler/Noll/Vo (FNV1a)</summary>
		[XmlEnumAttribute("FNV1A-32")]
		FNV1A32,

		/// <summary>64bit Fowler/Noll/Vo (FNV1a)</summary>
		[XmlEnumAttribute("FNV1A-64")]
		FNV1A64,

		/// <summary>GHash3</summary>
		[XmlEnumAttribute("GHASH-3")]
		GHASH3,

		/// <summary>GHash5</summary>
		[XmlEnumAttribute("GHASH-5")]
		GHASH5,

		/// <summary>GOST Hash</summary>
		GOSTHASH,

		/// <summary>HAVAL (3 pass, 128bit)</summary>
		[XmlEnumAttribute("HAVAL-3-128")]
		HAVAL3128,

		/// <summary>HAVAL (3 pass, 160bit)</summary>
		[XmlEnumAttribute("HAVAL-3-160")]
		HAVAL3160,

		/// <summary>HAVAL (3 pass, 192bit)</summary>
		[XmlEnumAttribute("HAVAL-3-192")]
		HAVAL3192,

		/// <summary>HAVAL (3 pass, 224bit)</summary>
		[XmlEnumAttribute("HAVAL-3-224")]
		HAVAL3224,

		/// <summary>HAVAL (3 pass, 256bit)</summary>
		[XmlEnumAttribute("HAVAL-3-256")]
		HAVAL3256,

		/// <summary>HAVAL (4 pass, 128bit)</summary>
		[XmlEnumAttribute("HAVAL-4-128")]
		HAVAL4128,

		/// <summary>HAVAL (4 pass, 160bit)</summary>
		[XmlEnumAttribute("HAVAL-4-160")]
		HAVAL4160,

		/// <summary>HAVAL (4 pass, 192bit)</summary>
		[XmlEnumAttribute("HAVAL-4-192")]
		HAVAL4192,

		/// <summary>HAVAL (4 pass, 224bit)</summary>
		[XmlEnumAttribute("HAVAL-4-224")]
		HAVAL4224,

		/// <summary>HAVAL (4 pass, 256bit)</summary>
		[XmlEnumAttribute("HAVAL-4-256")]
		HAVAL4256,

		/// <summary>HAVAL (5 pass, 128bit)</summary>
		[XmlEnumAttribute("HAVAL-5-128")]
		HAVAL5128,

		/// <summary>HAVAL (5 pass, 160bit)</summary>
		[XmlEnumAttribute("HAVAL-5-160")]
		HAVAL5160,

		/// <summary>HAVAL (5 pass, 192bit)</summary>
		[XmlEnumAttribute("HAVAL-5-192")]
		HAVAL5192,

		/// <summary>HAVAL (5 pass, 224bit)</summary>
		[XmlEnumAttribute("HAVAL-5-224")]
		HAVAL5224,

		/// <summary>HAVAL (5 pass, 256bit)</summary>
		[XmlEnumAttribute("HAVAL-5-256")]
		HAVAL5256,

		/// <summary>Jenkins Hash</summary>
		JHASH,

		/// <summary>Message Digest 2</summary>
		MD2,

		/// <summary>Message Digest 4</summary>
		MD4,

		/// <summary>Message Digest 5</summary>
		MD5,

		/// <summary>128bit RIPE-MD</summary>
		RIPEMD128,

		/// <summary>160bit RIPE-MD</summary>
		RIPEMD160,

		/// <summary>256bit RIPE-MD</summary>
		RIPEMD256,

		/// <summary>320bit RIPE-MD</summary>
		RIPEMD320,

		/// <summary>SHA0</summary>
		SHA0,

		/// <summary>SHA1</summary>
		SHA1,

		/// <summary>SHA2 (224bit)</summary>
		SHA224,

		/// <summary>SHA2 (256bit)</summary>
		SHA256,

		/// <summary>SHA2 (384bit)</summary>
		SHA384,

		/// <summary>SHA2 (512bit)</summary>
		SHA512,

		/// <summary>Snefru2 (4 pass, 128bit)</summary>
		[XmlEnumAttribute("SNEFRU2-4-128")]
		SNEFRU24128,

		/// <summary>Snefru2 (4 pass, 256bit)</summary>
		[XmlEnumAttribute("SNEFRU2-4-256")]
		SNEFRU24256,

		/// <summary>Snefru2 (8 pass, 128bit)</summary>
		[XmlEnumAttribute("SNEFRU2-8-128")]
		SNEFRU28128,

		/// <summary>Snefru2 (8 pass, 256bit)</summary>
		[XmlEnumAttribute("SNEFRU2-8-256")]
		SNEFRU28256,

		/// <summary>Sum (BSD)</summary>
		[XmlEnumAttribute("SUM-BSD")]
		SUMBSD,

		/// <summary>Sum (SysV)</summary>
		[XmlEnumAttribute("SUM-SYSV")]
		SUMSYSV,

		/// <summary>Tiger</summary>
		TIGER,

		/// <summary>Whirlpool</summary>
		WHIRLPOOL,

		/// <summary>XUM32</summary>
		XUM32
	}
}
