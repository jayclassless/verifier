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
using System.IO;
using System.Collections;
using System.Security.Cryptography;

namespace Classless.Verifier {
	/// <summary>.</summary>
	public class HashCalculator {
		private Hashtable hashers;
		private int bufferSize;


		/// <summary>Gets or sets the buffer size in bytes that is used when computing streams.</summary>
		public int BufferSize {
			get { return bufferSize; }
			set {
				if (value <= 0) {
					throw new ArgumentOutOfRangeException("value", value, "BufferSize must be greater than zero bytes.");
				} else {
					bufferSize = value;
				}
			}
		}


		/// <summary>Initializes an instance of Calculator.</summary>
		public HashCalculator() {
			hashers = new Hashtable();
			bufferSize = 4096;
		}


		/// <summary>Adds a new algorithm into the processing.</summary>
		/// <param name="type">The algorithm to add.</param>
		public void AddAlgorithm(AlgorithmType type) {
			if (!hashers.ContainsKey(type)) {
				hashers.Add(type, Algorithm.GetHasherFromType(type));
			}
		}


		/// <summary>Removes an algorithm from the processing.</summary>
		/// <param name="type">The algorithm to remove.</param>
		public void RemoveAlgorithm(AlgorithmType type) {
			if (hashers.ContainsKey(type)) {
				hashers.Remove(type);
			}
		}


		/// <summary>Get the result hash/checksum for a given algorithm.</summary>
		/// <param name="type">The algorithm to retrieve the result for.</param>
		/// <returns>The hash/checksum.</returns>
		public byte[] GetHash(AlgorithmType type) {
			return ((HashAlgorithm)hashers[type]).Hash;
		}


		/// <summary>Computes the hash value for the specified region of the specified byte array.</summary>
		/// <param name="buffer">The input for which to compute the hash code.</param>
		/// <param name="offset">The position in the array to start reading from.</param>
		/// <param name="count">The number of bytes to read in the array.</param>
		public void ComputeHash(byte[] buffer, int offset, int count) {
			foreach(HashAlgorithm h in hashers.Values) {
				h.ComputeHash(buffer, offset, count);
			}
		}


		/// <summary>Computes the hash value for the specified byte array.</summary>
		/// <param name="buffer">The input for which to compute the hash code.</param>
		public void ComputeHash(byte[] buffer) {
			ComputeHash(buffer, 0, buffer.Length);
		}


		/// <summary>Computes the hash value for the specified string.</summary>
		/// <param name="data">The input for which to compute the hash code.</param>
		/// <remarks>Assumes ASCII encoding.</remarks>
		public void ComputeHash(string data) {
			byte[] temp = System.Text.Encoding.ASCII.GetBytes(data);
			ComputeHash(temp);
		}


		/// <summary>Computes the hash value for the specified stream.</summary>
		/// <param name="inputStream">The input for which to compute the hash code.</param>
		public void ComputeHash(Stream inputStream) {
			byte[] buffer = new byte[bufferSize];
			byte[] temp = new byte[bufferSize];
			int bytesRead;

			Array.Copy(buffer, temp, bufferSize);
			do {
				bytesRead = inputStream.Read(buffer, 0, bufferSize);
				if (bytesRead == bufferSize) {
					foreach(HashAlgorithm h in hashers.Values) {
						h.TransformBlock(buffer, 0, bufferSize, temp, 0);
					}
				} else {
					foreach(HashAlgorithm h in hashers.Values) {
						h.TransformFinalBlock(buffer, 0, bytesRead);
					}
				}
			} while (bytesRead == bufferSize);
		}
	}
}
