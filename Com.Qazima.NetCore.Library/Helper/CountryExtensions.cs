using System;
using System.Globalization;

namespace Com.Qazima.NetCore.Library.Helper {
    public static class CountryExtensions {
        public static CountryCodeAlpha2 ToAlpha2(this CountryCodeAlpha3 countryCode) {
            return (CountryCodeAlpha2)countryCode;
        }

        public static CountryCodeAlpha2 ToAlpha2(this CultureInfo cultureInfo) {
            return new RegionInfo(cultureInfo.Name).ToAlpha2();
        }

        public static CountryCodeAlpha2 ToAlpha2(this RegionInfo regionInfo) {
            return (CountryCodeAlpha2)Enum.Parse(typeof(CountryCodeAlpha2), regionInfo.TwoLetterISORegionName);
        }

        public static CountryCodeAlpha3 ToAlpha3(this CountryCodeAlpha2 countryCode) {
            return (CountryCodeAlpha3)countryCode;
        }

        public static CountryCodeAlpha3 ToAlpha3(this CultureInfo cultureInfo) {
            return new RegionInfo(cultureInfo.Name).ToAlpha3();
        }

        public static CountryCodeAlpha3 ToAlpha3(this RegionInfo regionInfo) {
            return (CountryCodeAlpha3)Enum.Parse(typeof(CountryCodeAlpha3), regionInfo.ThreeLetterISORegionName);
        }
    }
}
