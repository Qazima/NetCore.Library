using System;
using System.Collections.Generic;

namespace Com.Qazima.NetCore.Library.Helper
{
    public static class DateExtensions
    {
        public static List<DateTime> GetSpecialDates(int year, CountryCodeAlpha2 countryCode, params SpecialDate[] specialDates)
        {
            return GetSpecialDates(year, countryCode.ToAlpha3(), specialDates);
        }

        public static List<DateTime> GetSpecialDates(int year, CountryCodeAlpha3 countryCode, params SpecialDate[] specialDates)
        {
            List<DateTime> result = new List<DateTime>();

            foreach (SpecialDate specialDate in specialDates)
            {
                switch (specialDate)
                {
                    case SpecialDate.Christmas:
                        {
                            result.Add(new DateTime(year, 12, 25));
                        }
                        break;
                    case SpecialDate.ChristmasEve:
                        {
                            result.Add(new DateTime(year, 12, 24));
                        }
                        break;
                    case SpecialDate.Easter:
                        {
                            int n = year % 19;
                            int c = year / 100;
                            int u = year % 100;
                            int s = c / 4;
                            int t = c % 4;
                            int p = (c + 8) / 25;
                            int q = (c - p + 1) / 3;
                            int e = ((19 * n) + c - s - q + 15) % 30;
                            int b = u / 4;
                            int d = u % 4;
                            int L = ((2 * t) + (2 * b) - e - d + 32) % 7;
                            int h = (n + (11 * e) + (22 * L)) / 451;
                            int m = (e + L - (7 * h) + 114) / 31;
                            int j = (e + L - (7 * h) + 114) % 31;
                            result.Add(new DateTime(year, m, j + 1));
                        }
                        break;
                    case SpecialDate.NationalDays:
                        {
                            switch (countryCode)
                            {
                                case CountryCodeAlpha3.AFG:
                                    {
                                        result.Add(new DateTime(year, 8, 19));
                                    }
                                    break;
                                case CountryCodeAlpha3.ALB:
                                    {
                                        result.Add(new DateTime(year, 11, 28));
                                    }
                                    break;
                                case CountryCodeAlpha3.ATA:
                                    {
                                    }
                                    break;
                                case CountryCodeAlpha3.DZA:
                                    {
                                        result.Add(new DateTime(year, 7, 5));
                                        result.Add(new DateTime(year, 11, 1));
                                    }
                                    break;
                                case CountryCodeAlpha3.ASM:
                                    {
                                    }
                                    break;
                                case CountryCodeAlpha3.AND:
                                    {
                                        result.Add(new DateTime(year, 9, 8));
                                    }
                                    break;
                                case CountryCodeAlpha3.AGO:
                                    {
                                        result.Add(new DateTime(year, 11, 11));
                                    }
                                    break;
                                case CountryCodeAlpha3.ATG:
                                    {
                                        result.Add(new DateTime(year, 11, 1));
                                    }
                                    break;
                                case CountryCodeAlpha3.AZE:
                                    {
                                        result.Add(new DateTime(year, 5, 28));
                                    }
                                    break;
                                case CountryCodeAlpha3.ARG:
                                    {
                                        result.Add(new DateTime(year, 8, 9));
                                    }
                                    break;
                                case CountryCodeAlpha3.AUS:
                                    {
                                        result.Add(new DateTime(year, 1, 26));
                                    }
                                    break;
                                case CountryCodeAlpha3.AUT:
                                    {
                                        result.Add(new DateTime(year, 10, 26));
                                    }
                                    break;
                                case CountryCodeAlpha3.BHS:
                                    {
                                        result.Add(new DateTime(year, 7, 10));
                                    }
                                    break;
                                case CountryCodeAlpha3.BHR:
                                    {
                                        result.Add(new DateTime(year, 12, 16));
                                    }
                                    break;
                                case CountryCodeAlpha3.BGD:
                                    {
                                        result.Add(new DateTime(year, 3, 26));
                                    }
                                    break;
                                case CountryCodeAlpha3.ARM:
                                    {
                                        result.Add(new DateTime(year, 9, 21));
                                    }
                                    break;
                                case CountryCodeAlpha3.BRB:
                                    {
                                        result.Add(new DateTime(year, 11, 30));
                                    }
                                    break;
                                case CountryCodeAlpha3.BEL:
                                    {
                                        result.Add(new DateTime(year, 7, 21));
                                    }
                                    break;
                                case CountryCodeAlpha3.BMU:
                                    {
                                        result.Add(new DateTime(year, 5, 24));
                                    }
                                    break;
                                case CountryCodeAlpha3.BTN:
                                    {
                                        result.Add(new DateTime(year, 12, 17));
                                    }
                                    break;
                                case CountryCodeAlpha3.BOL:
                                    {
                                        result.Add(new DateTime(year, 8, 6));
                                    }
                                    break;
                                case CountryCodeAlpha3.BIH:
                                    {
                                        result.Add(new DateTime(year, 11, 21));
                                    }
                                    break;
                                case CountryCodeAlpha3.BWA:
                                    {
                                        result.Add(new DateTime(year, 9, 30));
                                    }
                                    break;
                                case CountryCodeAlpha3.BVT:
                                    {
                                    }
                                    break;
                                case CountryCodeAlpha3.BRA:
                                    {
                                        result.Add(new DateTime(year, 9, 7));
                                    }
                                    break;
                                case CountryCodeAlpha3.BLZ:
                                    {
                                        result.Add(new DateTime(year, 9, 21));
                                    }
                                    break;
                                case CountryCodeAlpha3.IOT:
                                    {
                                    }
                                    break;
                                case CountryCodeAlpha3.SLB:
                                    {
                                        result.Add(new DateTime(year, 7, 7));
                                    }
                                    break;
                                case CountryCodeAlpha3.VGB:
                                    {
                                    }
                                    break;
                                case CountryCodeAlpha3.BRN:
                                    {
                                        result.Add(new DateTime(year, 2, 23));
                                    }
                                    break;
                                case CountryCodeAlpha3.BGR:
                                    {
                                        result.Add(new DateTime(year, 3, 3));
                                    }
                                    break;
                                case CountryCodeAlpha3.MMR:
                                    {
                                        result.Add(new DateTime(year, 1, 4));
                                    }
                                    break;
                                case CountryCodeAlpha3.BDI:
                                    {
                                        result.Add(new DateTime(year, 7, 1));
                                    }
                                    break;
                                case CountryCodeAlpha3.BLR:
                                    {
                                        result.Add(new DateTime(year, 7, 3));
                                    }
                                    break;
                                case CountryCodeAlpha3.KHM:
                                    {
                                        result.Add(new DateTime(year, 11, 9));
                                    }
                                    break;
                                case CountryCodeAlpha3.CMR:
                                    {
                                        result.Add(new DateTime(year, 5, 20));
                                    }
                                    break;
                                case CountryCodeAlpha3.CAN:
                                    {
                                        result.Add(new DateTime(year, 7, 1));
                                    }
                                    break;
                                case CountryCodeAlpha3.CPV:
                                    {
                                        result.Add(new DateTime(year, 7, 5));
                                    }
                                    break;
                                case CountryCodeAlpha3.CYM:
                                    {
                                        DateTime temp = new DateTime(year, 7, 1);
                                        switch (temp.DayOfWeek)
                                        {
                                            case DayOfWeek.Sunday:
                                                temp = temp.AddDays(1);
                                                break;
                                            case DayOfWeek.Monday:
                                                break;
                                            default:
                                                temp = temp.AddDays(8 - (int)temp.DayOfWeek);
                                                break;
                                        }
                                        result.Add(temp);
                                    }
                                    break;
                                case CountryCodeAlpha3.CAF:
                                    {
                                        result.Add(new DateTime(year, 12, 1));
                                    }
                                    break;
                                case CountryCodeAlpha3.LKA:
                                    {
                                        result.Add(new DateTime(year, 2, 4));
                                    }
                                    break;
                                case CountryCodeAlpha3.TCD:
                                    {
                                        result.Add(new DateTime(year, 8, 11));
                                    }
                                    break;
                                case CountryCodeAlpha3.CHL:
                                    {
                                        result.Add(new DateTime(year, 9, 18));
                                    }
                                    break;
                                case CountryCodeAlpha3.CHN:
                                    {
                                        result.Add(new DateTime(year, 10, 1));
                                        result.Add(new DateTime(year, 10, 2));
                                    }
                                    break;
                                case CountryCodeAlpha3.TWN:
                                    {
                                        result.Add(new DateTime(year, 10, 10));
                                    }
                                    break;
                                case CountryCodeAlpha3.CXR:
                                    {
                                    }
                                    break;
                                case CountryCodeAlpha3.CCK:
                                    {
                                    }
                                    break;
                                case CountryCodeAlpha3.COL:
                                    {
                                        result.Add(new DateTime(year, 7, 20));
                                    }
                                    break;
                                case CountryCodeAlpha3.COM:
                                    {
                                        result.Add(new DateTime(year, 7, 6));
                                    }
                                    break;
                                case CountryCodeAlpha3.MYT:
                                    {
                                    }
                                    break;
                                case CountryCodeAlpha3.COG:
                                    {
                                        result.Add(new DateTime(year, 8, 15));
                                    }
                                    break;
                                case CountryCodeAlpha3.COD:
                                    {
                                        result.Add(new DateTime(year, 6, 30));
                                    }
                                    break;
                                case CountryCodeAlpha3.COK:
                                    {
                                        result.Add(new DateTime(year, 8, 4));
                                    }
                                    break;
                                case CountryCodeAlpha3.CRI:
                                    {
                                        result.Add(new DateTime(year, 9, 15));
                                    }
                                    break;
                                case CountryCodeAlpha3.HRV:
                                    {
                                        result.Add(new DateTime(year, 6, 25));
                                    }
                                    break;
                                case CountryCodeAlpha3.CUB:
                                    {
                                        result.Add(new DateTime(year, 12, 10));
                                    }
                                    break;
                                case CountryCodeAlpha3.CYP:
                                    {
                                        result.Add(new DateTime(year, 10, 1));
                                    }
                                    break;
                                case CountryCodeAlpha3.CZE:
                                    {
                                        result.Add(new DateTime(year, 10, 28));
                                    }
                                    break;
                                case CountryCodeAlpha3.BEN:
                                    {
                                        result.Add(new DateTime(year, 8, 1));
                                    }
                                    break;
                                case CountryCodeAlpha3.DNK:
                                    {
                                        result.Add(new DateTime(year, 4, 16));
                                        result.Add(new DateTime(year, 6, 5));
                                    }
                                    break;
                                case CountryCodeAlpha3.DMA:
                                    {
                                        result.Add(new DateTime(year, 11, 3));
                                    }
                                    break;
                                case CountryCodeAlpha3.DOM:
                                    {
                                        result.Add(new DateTime(year, 2, 27));
                                    }
                                    break;
                                case CountryCodeAlpha3.ECU:
                                    {
                                        result.Add(new DateTime(year, 8, 10));
                                    }
                                    break;
                                case CountryCodeAlpha3.SLV:
                                    {
                                        result.Add(new DateTime(year, 9, 15));
                                    }
                                    break;
                                case CountryCodeAlpha3.GNQ:
                                    {
                                        result.Add(new DateTime(year, 10, 12));
                                    }
                                    break;
                                case CountryCodeAlpha3.ETH:
                                    {
                                        result.Add(new DateTime(year, 5, 28));
                                    }
                                    break;
                                case CountryCodeAlpha3.ERI:
                                    {
                                        result.Add(new DateTime(year, 5, 24));
                                    }
                                    break;
                                case CountryCodeAlpha3.EST:
                                    {
                                        result.Add(new DateTime(year, 2, 24));
                                    }
                                    break;
                                case CountryCodeAlpha3.FRO:
                                    {
                                        result.Add(new DateTime(year, 4, 16));
                                        result.Add(new DateTime(year, 6, 5));
                                    }
                                    break;
                                case CountryCodeAlpha3.FLK:
                                    {
                                    }
                                    break;
                                case CountryCodeAlpha3.SGS:
                                    {
                                    }
                                    break;
                                case CountryCodeAlpha3.FJI:
                                    {
                                        result.Add(new DateTime(year, 10, 10));
                                    }
                                    break;
                                case CountryCodeAlpha3.FIN:
                                    {
                                        result.Add(new DateTime(year, 12, 6));
                                    }
                                    break;
                                case CountryCodeAlpha3.ALA:
                                    {
                                        result.Add(new DateTime(year, 12, 6));
                                    }
                                    break;
                                case CountryCodeAlpha3.FRA:
                                    {
                                        result.Add(new DateTime(year, 7, 14));
                                    }
                                    break;
                                case CountryCodeAlpha3.GUF:
                                    {
                                        result.Add(new DateTime(year, 7, 14));
                                    }
                                    break;
                                case CountryCodeAlpha3.PYF:
                                    {
                                        result.Add(new DateTime(year, 7, 14));
                                    }
                                    break;
                                case CountryCodeAlpha3.ATF:
                                    {
                                        result.Add(new DateTime(year, 7, 14));
                                    }
                                    break;
                                case CountryCodeAlpha3.DJI:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GAB:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GEO:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GMB:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PSE:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.DEU:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GHA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GIB:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.KIR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GRC:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GRL:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GRD:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GLP:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GUM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GTM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GIN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GUY:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.HTI:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.HMD:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.VAT:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.HND:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.HKG:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.HUN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.ISL:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.IND:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.IDN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.IRN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.IRQ:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.IRL:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.ISR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.ITA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.CIV:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.JAM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.JPN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.KAZ:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.JOR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.KEN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PRK:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.KOR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.KWT:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.KGZ:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.LAO:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.LBN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.LSO:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.LVA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.LBR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.LBY:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.LIE:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.LTU:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.LUX:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MAC:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MDG:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MWI:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MYS:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MDV:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MLI:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MLT:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MTQ:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MRT:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MUS:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MEX:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MCO:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MNG:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MDA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MNE:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MSR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MAR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MOZ:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.OMN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NAM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NRU:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NPL:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NLD:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.CUW:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.ABW:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SXM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.BES:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NCL:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.VUT:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NZL:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NIC:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NER:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NGA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NIU:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NFK:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.NOR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MNP:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.UMI:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.FSM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MHL:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PLW:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PAK:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PAN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PNG:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PRY:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PER:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PHL:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PCN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.POL:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PRT:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GNB:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TLS:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.PRI:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.QAT:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.REU:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.ROU:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.RUS:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.RWA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.BLM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SHN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.KNA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.AIA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.LCA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MAF:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SPM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.VCT:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SMR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.STP:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SAU:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SEN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SRB:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SYC:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SLE:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SGP:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SVK:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.VNM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SVN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SOM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.ZAF:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.ZWE:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.ESP:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SSD:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SDN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.ESH:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SUR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SJM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SWZ:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SWE:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.CHE:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.SYR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TJK:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.THA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TGO:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TKL:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TON:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TTO:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.ARE:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TUN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TUR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TKM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TCA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TUV:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.UGA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.UKR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.MKD:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.EGY:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.GBR:
                                    {
                                    }
                                    break;
                                case CountryCodeAlpha3.GGY:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.JEY:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.IMN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.TZA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.USA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.VIR:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.BFA:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.URY:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.UZB:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.VEN:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.WLF:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.WSM:
                                    {
                                        result.Add(new DateTime(year, 6, 1));
                                    }
                                    break;
                                case CountryCodeAlpha3.YEM:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                case CountryCodeAlpha3.ZMB:
                                    {
                                        //result.Add(new DateTime(year, ));
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case SpecialDate.NewYearDay:
                        {
                            result.Add(new DateTime(year, 1, 1));
                        }
                        break;
                    case SpecialDate.NewYearDayEve:
                        {
                            result.Add(new DateTime(year, 12, 31));
                        }
                        break;
                    case SpecialDate.Pentecost:
                        {
                            int n = year % 19;
                            int c = year / 100;
                            int u = year % 100;
                            int s = c / 4;
                            int t = c % 4;
                            int p = (c + 8) / 25;
                            int q = (c - p + 1) / 3;
                            int e = ((19 * n) + c - s - q + 15) % 30;
                            int b = u / 4;
                            int d = u % 4;
                            int L = ((2 * t) + (2 * b) - e - d + 32) % 7;
                            int h = (n + (11 * e) + (22 * L)) / 451;
                            int m = (e + L - (7 * h) + 114) / 31;
                            int j = (e + L - (7 * h) + 114) % 31;
                            result.Add(new DateTime(year, m, j + 1).AddDays(49));
                        }
                        break;
                    default:
                        {
                            result.Add(DateTime.Now);
                        }
                        break;
                }
            }

            return result;
        }
    }
}
