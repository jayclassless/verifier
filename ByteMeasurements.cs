// $Id$

/*
 * Classless.Utilities.ByteMeasurements
 * A Byte Size and Rate Formatting Utility for C#/.NET
 * http://www.classless.net
 * 
 * Version 1.0, 7/5/2004
 * 
 * */

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
 * The Original Code is Classless.Utilities.ByteMeasurements.
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

namespace Classless.Utilities {
	/// <summary>Represents a unit system that bytes can be measured in.</summary>
	public enum MeasurementSystem {
		/// <summary>International System of Units (aka Metric System).</summary>
		SI,

		/// <summary>International Electrotechnical Commission Binary Units (IEC 60027-2).</summary>
		Binary
	}


	/// <summary>Represents the base units of the SI system.</summary>
	public enum UnitSI : long {
		/// <summary>1 Byte.</summary>
		Byte = 1,

		/// <summary>1 Kiloyte (10^3).</summary>
		Kilobyte = 1000,

		/// <summary>1 Megabyte (10^6).</summary>
		Megabyte = 1000000,

		/// <summary>1 Gigabyte (10^9).</summary>
		Gigabyte = 1000000000,

		/// <summary>1 Terabyte (10^12).</summary>
		Terabyte = 1000000000000,

		/// <summary>1 Petabyte (10^15).</summary>
		Petabyte = 1000000000000000,

		/// <summary>1 Exabyte (10^18).</summary>
		Exabyte = 1000000000000000000
	}


	/// <summary>Represents the base units of the standard IEC system.</summary>
	public enum UnitBinary : long {
		/// <summary>1 Byte.</summary>
		Byte = 1,

		/// <summary>1 Kibibyte (2^10).</summary>
		Kibibyte = 1024,

		/// <summary>1 Mebibyte (2^20).</summary>
		Mebibyte = 1048576,

		/// <summary>1 Gibibyte (2^30).</summary>
		Gibibyte = 1073741824,

		/// <summary>1 Tebibyte (2^40).</summary>
		Tebibyte = 1099511627776,

		/// <summary>1 Pebibyte (2^50).</summary>
		Pebibyte = 1125899906842624,

		/// <summary>1 Exbibyte (2^60).</summary>
		Exbibyte = 1152921504606846976
	}


	/// <summary>A class for measuring byte quantities.</summary>
	public class ByteMeasurements {
		private long numBytes = 0;


		/// <summary>Gets or sets the number of bytes the measuring is performed against.</summary>
		public long NumBytes {
			get { return numBytes; }
			set { numBytes = value; }
		}


		/// <summary>Initializes an instance of ByteMeasurements.</summary>
		public ByteMeasurements() {}
		/// <summary>Initializes an instance of ByteMeasurements.</summary>
		/// <param name="numBytes">The number of bytes to measure against.</param>
		public ByteMeasurements(long numBytes) { this.NumBytes = numBytes; }
		/// <summary>Initializes an instance of ByteMeasurements.</summary>
		/// <param name="numBytes">The number of bytes to measure against.</param>
		public ByteMeasurements(float numBytes) { this.NumBytes = (long)numBytes; }
		/// <summary>Initializes an instance of ByteMeasurements.</summary>
		/// <param name="numBytes">The number of bytes to measure against.</param>
		public ByteMeasurements(double numBytes) { this.NumBytes = (long)numBytes; }


		/// <summary>Reduce the number of bytes to its highest measureable unit.</summary>
		/// <returns>A formatted string containing the number of bytes and unit name.</returns>
		/// <remarks>Defaults to the non-abbreviated Binary system with 2 digits of precision.</remarks>
		public string Units() { return Units(MeasurementSystem.Binary, false, 2); }
		/// <summary>Reduce the number of bytes to its highest measureable unit.</summary>
		/// <param name="type">The system of measurement to use.</param>
		/// <returns>A formatted string containing the number of bytes and unit name.</returns>
		/// <remarks>Defaults to the non-abbreviated system with 2 digits of precision.</remarks>
		public string Units(MeasurementSystem type) { return Units(type, false, 2); }
		/// <summary>Reduce the number of bytes to its highest measureable unit.</summary>
		/// <param name="type">The system of measurement to use.</param>
		/// <param name="abbreviate">Whether or not to abbreviate the unit name.</param>
		/// <returns>A formatted string containing the number of bytes and unit name.</returns>
		/// <remarks>Defaults to 2 digits of precision.</remarks>
		public string Units(MeasurementSystem type, bool abbreviate) { return Units(type, abbreviate, 2); }
		/// <summary>Reduce the number of bytes to its highest measureable unit.</summary>
		/// <param name="type">The system of measurement to use.</param>
		/// <param name="abbreviate">Whether or not to abbreviate the unit name.</param>
		/// <param name="precision">The number of digits after the decimal point to show.</param>
		/// <returns>A formatted string containing the number of bytes and unit name.</returns>
		public string Units(MeasurementSystem type, bool abbreviate, int precision) {
			string temp = "n/a";

			try {
				temp = units(NumBytes, type, abbreviate, precision);
			} catch (ArithmeticException) {}

			return temp;
		}


		/// <summary>Reduce the number of bytes to its highest measureable unit per second.</summary>
		/// <param name="seconds">The number of seconds the amount is measured against.</param>
		/// <returns>A formatted string containing the number of bytes and unit name per second.</returns>
		public string Rate(double seconds) { return Rate(seconds, MeasurementSystem.Binary, false, 2); }
		/// <summary>Reduce the number of bytes to its highest measureable unit per second.</summary>
		/// <param name="seconds">The number of seconds the amount is measured against.</param>
		/// <param name="type">The system of measurement to use.</param>
		/// <returns>A formatted string containing the number of bytes and unit name per second.</returns>
		public string Rate(double seconds, MeasurementSystem type) { return Rate(seconds, type, false, 2); }
		/// <summary>Reduce the number of bytes to its highest measureable unit per second.</summary>
		/// <param name="seconds">The number of seconds the amount is measured against.</param>
		/// <param name="type">The system of measurement to use.</param>
		/// <param name="abbreviate">Whether or not to abbreviate the unit names.</param>
		/// <returns>A formatted string containing the number of bytes and unit name per second.</returns>
		public string Rate(double seconds, MeasurementSystem type, bool abbreviate) { return Rate(seconds, type, abbreviate, 2); }
		/// <summary>Reduce the number of bytes to its highest measureable unit per second.</summary>
		/// <param name="seconds">The number of seconds the amount is measured against.</param>
		/// <param name="type">The system of measurement to use.</param>
		/// <param name="abbreviate">Whether or not to abbreviate the unit names.</param>
		/// <param name="precision">The number of digits after the decimal point to show.</param>
		/// <returns>A formatted string containing the number of bytes and unit name per second.</returns>
		public string Rate(double seconds, MeasurementSystem type, bool abbreviate, int precision) {
			string temp = "n/a";

			try {
				temp = units((NumBytes / seconds), type, abbreviate, precision);
				if (abbreviate) {
					temp += "/s";
				} else {
					temp += "/second";
				}
			} catch (ArithmeticException) {}

			return temp;
		}


		/// <summary>Get the human-readable name for a specified unit.</summary>
		/// <param name="unit">The unit to get the name for.</param>
		/// <returns>The human-readable name for the specified unit.</returns>
		/// <remarks>Defaults to the non-abbreviated name.</remarks>
		static public string UnitName(UnitSI unit) { return UnitName(unit, false); }
		/// <summary>Get the human-readable name for a specified unit.</summary>
		/// <param name="unit">The unit to get the name for.</param>
		/// <param name="abbreviate">Whether or not to abbreviate the name.</param>
		/// <returns>The human-readable name for the specified unit.</returns>
		static public string UnitName(UnitSI unit, bool abbreviate) {
			string name;

			if (abbreviate) {
				if (unit == UnitSI.Byte) {
					name = "B";
				} else {
					name = unit.ToString().Substring(0, 1) + "B";
				}
			} else {
				name = unit.ToString();
			}

			return name;
		}


		/// <summary>Get the human-readable name for a specified unit.</summary>
		/// <param name="unit">The unit to get the name for.</param>
		/// <returns>The human-readable name for the specified unit.</returns>
		/// <remarks>Defaults to the non-abbreviated name.</remarks>
		static public string UnitName(UnitBinary unit) { return UnitName(unit, false); }
		/// <summary>Get the human-readable name for a specified unit.</summary>
		/// <param name="unit">The unit to get the name for.</param>
		/// <param name="abbreviate">Whether or not to abbreviate the name.</param>
		/// <returns>The human-readable name for the specified unit.</returns>
		static public string UnitName(UnitBinary unit, bool abbreviate) {
			string name;

			if (abbreviate) {
				if (unit == UnitBinary.Byte) {
					name = "B";
				} else {
					name = unit.ToString().Substring(0, 1) + "iB";
				}
			} else {
				name = unit.ToString();
			}

			return name;
		}


		// Internal function that does most of the math.
		private string units(double bytes, MeasurementSystem type, bool abbreviate, int precision) {
			string temp = string.Empty;

			if (type == MeasurementSystem.SI) {
				if (bytes < (long)UnitSI.Kilobyte) {
					if (Math.Floor(bytes) == bytes) {
						temp = bytes.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
					} else {
						temp = string.Format("{0:F" + precision.ToString() + "}", bytes);
					}
					temp +=  " " + UnitName(UnitSI.Byte, abbreviate);
				} else if (bytes < (long)UnitSI.Megabyte) {
					temp = string.Format("{0:F" + precision.ToString() + "}", (bytes / (double)UnitSI.Kilobyte)) + " " + UnitName(UnitSI.Kilobyte, abbreviate);
				} else if (bytes < (long)UnitSI.Gigabyte) {
					temp = string.Format("{0:F" + precision.ToString() + "}", (bytes / (double)UnitSI.Megabyte)) + " " + UnitName(UnitSI.Megabyte, abbreviate);
				} else if (bytes < (long)UnitSI.Terabyte) {
					temp = string.Format("{0:F" + precision.ToString() + "}", (bytes / (double)UnitSI.Gigabyte)) + " " + UnitName(UnitSI.Gigabyte, abbreviate);
				} else if (bytes < (long)UnitSI.Petabyte) {
					temp = string.Format("{0:F" + precision.ToString() + "}", (bytes / (double)UnitSI.Terabyte)) + " " + UnitName(UnitSI.Terabyte, abbreviate);
				} else if (bytes < (long)UnitSI.Exabyte) {
					temp = string.Format("{0:F" + precision.ToString() + "}", (bytes / (double)UnitSI.Petabyte)) + " " + UnitName(UnitSI.Petabyte, abbreviate);
				} else {
					// Make sure we have a "real" number before returning it.
					double check = (bytes / (double)UnitSI.Exabyte);
					if ((double.IsInfinity(check)) || (double.IsNaN(check)) || (double.IsNegativeInfinity(check)) || (double.IsPositiveInfinity(check))) {
						throw new ArithmeticException("Invalid result.");
					} else {
						temp = string.Format("{0:F" + precision.ToString() + "}", check) + " " + UnitName(UnitSI.Exabyte, abbreviate);
					}
				}
			} else if (type == MeasurementSystem.Binary) {
				if (bytes < (long)UnitBinary.Kibibyte) {
					if (Math.Floor(bytes) == bytes) {
						temp = bytes.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
					} else {
						temp = string.Format("{0:F" + precision.ToString() + "}", bytes);
					}
					temp += " " + UnitName(UnitBinary.Byte, abbreviate);
				} else if (bytes < (long)UnitBinary.Mebibyte) {
					temp = string.Format("{0:F" + precision.ToString() + "}", (bytes / (double)UnitBinary.Kibibyte)) + " " + UnitName(UnitBinary.Kibibyte, abbreviate);
				} else if (bytes < (long)UnitBinary.Gibibyte) {
					temp = string.Format("{0:F" + precision.ToString() + "}", (bytes / (double)UnitBinary.Mebibyte)) + " " + UnitName(UnitBinary.Mebibyte, abbreviate);
				} else if (bytes < (long)UnitBinary.Tebibyte) {
					temp = string.Format("{0:F" + precision.ToString() + "}", (bytes / (double)UnitBinary.Gibibyte)) + " " + UnitName(UnitBinary.Gibibyte, abbreviate);
				} else if (bytes < (long)UnitBinary.Pebibyte) {
					temp = string.Format("{0:F" + precision.ToString() + "}", (bytes / (double)UnitBinary.Tebibyte)) + " " + UnitName(UnitBinary.Tebibyte, abbreviate);
				} else if (bytes < (long)UnitBinary.Exbibyte) {
					temp = string.Format("{0:F" + precision.ToString() + "}", (bytes / (double)UnitBinary.Pebibyte)) + " " + UnitName(UnitBinary.Pebibyte, abbreviate);
				} else {
					// Make sure we have a "real" number before returning it.
					double check = (bytes / (double)UnitBinary.Exbibyte);
					if ((double.IsInfinity(check)) || (double.IsNaN(check)) || (double.IsNegativeInfinity(check)) || (double.IsPositiveInfinity(check))) {
						throw new ArithmeticException("Invalid result.");
					} else {
						temp = string.Format("{0:F" + precision.ToString() + "}", check) + " " + UnitName(UnitBinary.Exbibyte, abbreviate);
					}
				}
			} else {
				throw new System.NotSupportedException(type.ToString() + " is not supported.");
			}

			return temp;
		}
	}
}
