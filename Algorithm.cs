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

namespace Classless.Verifier {
	/// <summary>Summary description for Algorithms.</summary>
	public class Algorithm {
		/// <summary>Associates algorithm types with their names.</summary>
		static public AlgorithmName[] Names = new AlgorithmName[] {
			new AlgorithmName(AlgorithmType.ADLER32, "Adler-32", "Adler32"),
			new AlgorithmName(AlgorithmType.CKSUM, "Cksum", "Cksum"),
			new AlgorithmName(AlgorithmType.CRC8, "CRC (8bit)", "CRC"),
			new AlgorithmName(AlgorithmType.CRC8REVERSED, "CRC (8bit Rev.)", "CRC"),
			new AlgorithmName(AlgorithmType.CRC16, "CRC (16bit)", "CRC"),
			new AlgorithmName(AlgorithmType.CRC16ARC, "CRC (16bit ARC)", "CRC"),
			new AlgorithmName(AlgorithmType.CRC16CCITT, "CRC (16bit CCITT)", "CRC"),
			new AlgorithmName(AlgorithmType.CRC16CCITTREVERSED, "CRC (16bit CCITT Rev.)", "CRC"),
			new AlgorithmName(AlgorithmType.CRC16REVERSED, "CRC (16bit Rev.)", "CRC"),
			new AlgorithmName(AlgorithmType.CRC16ZMODEM, "CRC (16bit ZMODEM)", "CRC"),
			new AlgorithmName(AlgorithmType.CRC32, "CRC (32bit)", "CRC"),
			new AlgorithmName(AlgorithmType.CRC32BZIP2, "CRC (32bit BZip2)", "CRC"),
			new AlgorithmName(AlgorithmType.CRC32JAMCRC, "CRC (32bit JAMCRC)", "CRC"),
			new AlgorithmName(AlgorithmType.CRC32REVERSED, "CRC (32bit Rev.)", "CRC"),
			new AlgorithmName(AlgorithmType.ELFHASH, "ELF Hash", "ELF"),
			new AlgorithmName(AlgorithmType.FCS16, "FCS (16bit)", "FCS"),
			new AlgorithmName(AlgorithmType.FCS32, "FCS (32bit)", "FCS"),
			new AlgorithmName(AlgorithmType.FNV032, "FNV-0 (32bit)", "FNV"),
			new AlgorithmName(AlgorithmType.FNV064, "FNV-0 (64bit)", "FNV"),
			new AlgorithmName(AlgorithmType.FNV132, "FNV-1 (32bit)", "FNV"),
			new AlgorithmName(AlgorithmType.FNV164, "FNV-1 (64bit)", "FNV"),
			new AlgorithmName(AlgorithmType.FNV1A32, "FNV-1a (32bit)", "FNV"),
			new AlgorithmName(AlgorithmType.FNV1A64, "FNV-1a (64bit)", "FNV"),
			new AlgorithmName(AlgorithmType.GHASH3, "GHash-3", "GHash"),
			new AlgorithmName(AlgorithmType.GHASH5, "GHash-5", "GHash"),
			new AlgorithmName(AlgorithmType.GOSTHASH, "GOSTHash", "GOST"),
			new AlgorithmName(AlgorithmType.HAVAL3128, "HAVAL (3 / 128bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL3160, "HAVAL (3 / 160bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL3192, "HAVAL (3 / 192bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL3224, "HAVAL (3 / 224bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL3256, "HAVAL (3 / 256bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL4128, "HAVAL (4 / 128bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL4160, "HAVAL (4 / 160bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL4192, "HAVAL (4 / 192bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL4224, "HAVAL (4 / 224bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL4256, "HAVAL (4 / 256bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL5128, "HAVAL (5 / 128bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL5160, "HAVAL (5 / 160bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL5192, "HAVAL (5 / 192bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL5224, "HAVAL (5 / 224bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.HAVAL5256, "HAVAL (5 / 256bit)", "HAVAL"),
			new AlgorithmName(AlgorithmType.JHASH, "Jenkins Hash", "JHash"),
			new AlgorithmName(AlgorithmType.MD2, "MD2", "MD2"),
			new AlgorithmName(AlgorithmType.MD4, "MD4", "MD4"),
			new AlgorithmName(AlgorithmType.MD5, "MD5", "MD5"),
			new AlgorithmName(AlgorithmType.RIPEMD128, "RIPEMD (128bit)", "RIPEMD"),
			new AlgorithmName(AlgorithmType.RIPEMD160, "RIPEMD (160bit)", "RIPEMD"),
			new AlgorithmName(AlgorithmType.RIPEMD256, "RIPEMD (256bit)", "RIPEMD"),
			new AlgorithmName(AlgorithmType.RIPEMD320, "RIPEMD (320bit)", "RIPEMD"),
			new AlgorithmName(AlgorithmType.SHA0, "SHA0", "SHA0"),
			new AlgorithmName(AlgorithmType.SHA1, "SHA1", "SHA1"),
			new AlgorithmName(AlgorithmType.SHA224, "SHA224", "SHA2"),
			new AlgorithmName(AlgorithmType.SHA256, "SHA256", "SHA2"),
			new AlgorithmName(AlgorithmType.SHA384, "SHA384", "SHA2"),
			new AlgorithmName(AlgorithmType.SHA512, "SHA512", "SHA2"),
			new AlgorithmName(AlgorithmType.SNEFRU24128, "Snefru2 (4 / 128bit)", "Snefru2"),
			new AlgorithmName(AlgorithmType.SNEFRU28128, "Snefru2 (8 / 128bit)", "Snefru2"),
			new AlgorithmName(AlgorithmType.SNEFRU24256, "Snefru2 (4 / 256bit)", "Snefru2"),
			new AlgorithmName(AlgorithmType.SNEFRU28256, "Snefru2 (8 / 256bit)", "Snefru2"),
			new AlgorithmName(AlgorithmType.SUMBSD, "Sum (BSD)", "Sum"),
			new AlgorithmName(AlgorithmType.SUMSYSV, "Sum (SysV)", "Sum"),
			new AlgorithmName(AlgorithmType.TIGER, "Tiger", "Tiger"),
			new AlgorithmName(AlgorithmType.WHIRLPOOL, "Whirlpool", "Whirlpool"),
			new AlgorithmName(AlgorithmType.XUM32, "XUM32", "XUM32")
		};


		/// <summary>Get the name for a given AlgorithmType.</summary>
		/// <param name="type">The AlgorithmType to derive the name from.</param>
		/// <returns>The corresponding algorithm name.</returns>
		static public string GetNameFromType(AlgorithmType type) {
			foreach (AlgorithmName an in Names) {
				if (type == an.Type) {
					return an.Name;
				}
			}

			throw new System.NotSupportedException("Unsupported algorithm type.");
		}


		/// <summary>Get the short name for a given AlgorithmType.</summary>
		/// <param name="type">The AlgorithmType to derive the short name from.</param>
		/// <returns>The corresponding short algorithm name.</returns>
		static public string GetShortNameFromType(AlgorithmType type) {
			foreach (AlgorithmName an in Names) {
				if (type == an.Type) {
					return an.ShortName;
				}
			}

			throw new System.NotSupportedException("Unsupported algorithm type.");
		}


		/// <summary>Get the AlgorithmType given a name.</summary>
		/// <param name="name">The name to derive an AlgorithmType from.</param>
		/// <returns>The corresponding AlgorithmType.</returns>
		static public AlgorithmType GetTypeFromName(string name) {
			foreach (AlgorithmName an in Names) {
				if (name == an.Name) {
					return an.Type;
				}
			}

			throw new System.NotSupportedException("Unsupported algorithm type.");
		}


		/// <summary>Retrieve the appropriate HashAlgorithm for a given AlgorithmType.</summary>
		/// <param name="type">The AlgorithmType to derive a HashAlgorithm from.</param>
		/// <returns>The corresponding HashAlgorithm.</returns>
		static public System.Security.Cryptography.HashAlgorithm GetHasherFromType(AlgorithmType type) {
			switch (type) {
				case AlgorithmType.ADLER32: return new Classless.Hasher.Adler32();
				case AlgorithmType.CKSUM: return new Classless.Hasher.Cksum();
				case AlgorithmType.CRC16: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC16));
				case AlgorithmType.CRC16ARC: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC16_ARC));
				case AlgorithmType.CRC16CCITT: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC16_CCITT));
				case AlgorithmType.CRC16CCITTREVERSED: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC16_CCITT_REVERSED));
				case AlgorithmType.CRC16REVERSED: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC16_REVERSED));
				case AlgorithmType.CRC16ZMODEM: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC16_ZMODEM));
				case AlgorithmType.CRC32: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC32));
				case AlgorithmType.CRC32BZIP2: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC32_BZIP2));
				case AlgorithmType.CRC32JAMCRC: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC32_JAMCRC));
				case AlgorithmType.CRC32REVERSED: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC32_REVERSED));
				case AlgorithmType.CRC8: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC8));
				case AlgorithmType.CRC8REVERSED: return new Classless.Hasher.CRC(Classless.Hasher.CRCParameters.GetParameters(Classless.Hasher.CRCStandard.CRC8_REVERSED));
				case AlgorithmType.ELFHASH: return new Classless.Hasher.ElfHash();
				case AlgorithmType.FCS16: return new Classless.Hasher.FCS16();
				case AlgorithmType.FCS32: return new Classless.Hasher.FCS32();
				case AlgorithmType.FNV032: return new Classless.Hasher.FNV(Classless.Hasher.FNVParameters.GetParameters(Classless.Hasher.FNVStandard.FNV0_32));
				case AlgorithmType.FNV064: return new Classless.Hasher.FNV(Classless.Hasher.FNVParameters.GetParameters(Classless.Hasher.FNVStandard.FNV0_64));
				case AlgorithmType.FNV132: return new Classless.Hasher.FNV(Classless.Hasher.FNVParameters.GetParameters(Classless.Hasher.FNVStandard.FNV1_32));
				case AlgorithmType.FNV164: return new Classless.Hasher.FNV(Classless.Hasher.FNVParameters.GetParameters(Classless.Hasher.FNVStandard.FNV1_64));
				case AlgorithmType.FNV1A32: return new Classless.Hasher.FNV(Classless.Hasher.FNVParameters.GetParameters(Classless.Hasher.FNVStandard.FNV1A_32));
				case AlgorithmType.FNV1A64: return new Classless.Hasher.FNV(Classless.Hasher.FNVParameters.GetParameters(Classless.Hasher.FNVStandard.FNV1A_64));
				case AlgorithmType.GHASH3: return new Classless.Hasher.GHash(Classless.Hasher.GHashParameters.GetParameters(Classless.Hasher.GHashStandard.GHash_3));
				case AlgorithmType.GHASH5: return new Classless.Hasher.GHash(Classless.Hasher.GHashParameters.GetParameters(Classless.Hasher.GHashStandard.GHash_5));
				case AlgorithmType.GOSTHASH: return new Classless.Hasher.GOSTHash();
				case AlgorithmType.HAVAL3128: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_3_128));
				case AlgorithmType.HAVAL3160: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_3_160));
				case AlgorithmType.HAVAL3192: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_3_192));
				case AlgorithmType.HAVAL3224: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_3_224));
				case AlgorithmType.HAVAL3256: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_3_256));
				case AlgorithmType.HAVAL4128: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_4_128));
				case AlgorithmType.HAVAL4160: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_4_160));
				case AlgorithmType.HAVAL4192: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_4_192));
				case AlgorithmType.HAVAL4224: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_4_224));
				case AlgorithmType.HAVAL4256: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_4_256));
				case AlgorithmType.HAVAL5128: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_5_128));
				case AlgorithmType.HAVAL5160: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_5_160));
				case AlgorithmType.HAVAL5192: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_5_192));
				case AlgorithmType.HAVAL5224: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_5_224));
				case AlgorithmType.HAVAL5256: return new Classless.Hasher.HAVAL(Classless.Hasher.HAVALParameters.GetParameters(Classless.Hasher.HAVALStandard.HAVAL_5_256));
				case AlgorithmType.JHASH: return new Classless.Hasher.JHash();
				case AlgorithmType.MD2: return new Classless.Hasher.MD2();
				case AlgorithmType.MD4: return new Classless.Hasher.MD4();
				case AlgorithmType.MD5: return new Classless.Hasher.MD5();
				case AlgorithmType.RIPEMD128: return new Classless.Hasher.RIPEMD128();
				case AlgorithmType.RIPEMD160: return new Classless.Hasher.RIPEMD160();
				case AlgorithmType.RIPEMD256: return new Classless.Hasher.RIPEMD256();
				case AlgorithmType.RIPEMD320: return new Classless.Hasher.RIPEMD320();
				case AlgorithmType.SHA0: return new Classless.Hasher.SHA0();
				case AlgorithmType.SHA1: return new Classless.Hasher.SHA1();
				case AlgorithmType.SHA224: return new Classless.Hasher.SHA224();
				case AlgorithmType.SHA256: return new Classless.Hasher.SHA256();
				case AlgorithmType.SHA384: return new Classless.Hasher.SHA384();
				case AlgorithmType.SHA512: return new Classless.Hasher.SHA512();
				case AlgorithmType.SNEFRU24128: return new Classless.Hasher.Snefru2(Classless.Hasher.Snefru2Parameters.GetParameters(Classless.Hasher.Snefru2Standard.Snefru2_4_128));
				case AlgorithmType.SNEFRU28128: return new Classless.Hasher.Snefru2(Classless.Hasher.Snefru2Parameters.GetParameters(Classless.Hasher.Snefru2Standard.Snefru2_8_128));
				case AlgorithmType.SNEFRU24256: return new Classless.Hasher.Snefru2(Classless.Hasher.Snefru2Parameters.GetParameters(Classless.Hasher.Snefru2Standard.Snefru2_4_256));
				case AlgorithmType.SNEFRU28256: return new Classless.Hasher.Snefru2(Classless.Hasher.Snefru2Parameters.GetParameters(Classless.Hasher.Snefru2Standard.Snefru2_8_256));
				case AlgorithmType.SUMBSD: return new Classless.Hasher.SumBSD();
				case AlgorithmType.SUMSYSV: return new Classless.Hasher.SumSysV();
				case AlgorithmType.TIGER: return new Classless.Hasher.Tiger();
				case AlgorithmType.WHIRLPOOL: return new Classless.Hasher.Whirlpool();
				case AlgorithmType.XUM32: return new Classless.Hasher.XUM32();
				default: throw new System.NotSupportedException("Unsupported algorithm type.");
			}
		}


		/// <summary>Retrieve the appropriate HashAlgorithm for a given AlgorithmType name.</summary>
		/// <param name="name">The AlgorithmType name to derive a HashAlgorithm from.</param>
		/// <returns>The corresponding HashAlgorithm.</returns>
		static public System.Security.Cryptography.HashAlgorithm GetHasherFromName(string name) {
			return GetHasherFromType(GetTypeFromName(name));
		}
	}
}
