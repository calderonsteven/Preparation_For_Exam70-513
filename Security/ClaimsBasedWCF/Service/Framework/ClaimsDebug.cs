using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Web;
using LeastPrivilege.IdentityModel.Extensions;

namespace LeastPrivilege.IdentityModel.Debug
{
    public static class ClaimsDebug
    {
        public static void ShowClaims(ClaimSet claimSet)
        {
            ShowClaims(new List<ClaimSet> { claimSet }, false);
        }

        public static void ShowClaims(ClaimSet claimSet, bool verbose)
        {
            ShowClaims(new List<ClaimSet> { claimSet }, verbose);
        }

        public static void ShowClaims(IEnumerable<ClaimSet> claimSets)
        {
            ShowClaims(claimSets, false);
        }

        public static void ShowClaims(IEnumerable<ClaimSet> claimSets, bool verbose)
        {
            int count = 0;
            foreach (ClaimSet set in claimSets)
            {
                Heading(String.Format("Claim Set #{0}", ++count), ConsoleColor.Yellow);
                ShowClaimSet(set, false, verbose);
            }
        }

        public static void ShowClaimsHtml(ClaimSet claimSet)
        {
            ShowClaimsHtml(new List<ClaimSet> { claimSet }, false);
        }

        public static void ShowClaimsHtml(ClaimSet claimSet, bool verbose)
        {
            ShowClaimsHtml(new List<ClaimSet> { claimSet }, verbose);
        }

        public static void ShowClaimsHtml(IEnumerable<ClaimSet> claimSets)
        {
            ShowClaimsHtml(claimSets, false);
        }

        public static void ShowClaimsHtml(IEnumerable<ClaimSet> claimSets, bool verbose)
        {
            int count = 0;
            foreach (ClaimSet set in claimSets)
            {
                HeadingHtml(String.Format("Claim Set #{0}", ++count));
                ShowClaimSetHtml(set, false, verbose);
            }
        }

        private static void ShowClaimSet(ClaimSet set, bool isIssuer, bool verbose)
        {
            if (set.HasIssuer())
            {
                ShowClaimSet(set.Issuer, true, verbose);
            }

            string setType = set.GetType().Name;
            string setName = isIssuer ? "Issuer" : "Issued";

            Heading(String.Format("\n{0} Claims ({1})\n", setName, setType), 
                ConsoleColor.Green);

            if (set.Count == 0)
            {
                Console.WriteLine("(anonymous)\n");
            }

            foreach (Claim claim in set)
            {
                if (claim.Right.Equals(Rights.Identity))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine(claim.ClaimType);

                if (verbose)
                {
                    Console.WriteLine(claim.Resource.GetType().FullName);
                    Console.WriteLine(claim.Stringify());
                }
                else
                {
                    Console.WriteLine(claim.Resource);
                }
                
                Console.WriteLine(claim.Right);
                Console.WriteLine();

                Console.ResetColor();
            }
        }

        private static void ShowClaimSetHtml(ClaimSet set, bool isIssuer, bool verbose)
        {
            if (set.HasIssuer())
            {
                ShowClaimSetHtml(set.Issuer, true, verbose);
            }

            string setType = set.GetType().Name;
            string setName = isIssuer ? "Issuer" : "Issued";

            HeadingHtml(String.Format("\n{0} Claims ({1})\n", setName, setType));

            foreach (Claim claim in set)
            {
                bool isIdentity = claim.Right.Equals(Rights.Identity);

                WriteLineHtml(claim.ClaimType, isIdentity);

                if (verbose)
                {
                    WriteLineHtml(claim.Resource.GetType().FullName, isIdentity);
                    WriteLineHtml(claim.Stringify(), isIdentity);
                }
                else
                {
                    WriteLineHtml(claim.Resource.ToString(), isIdentity);
                }

                WriteLineHtml(claim.Right, isIdentity);
                WriteLineHtml("", false);

                Console.ResetColor();
            }
        }

        private static void Heading(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.WriteLine();
            Console.ResetColor();
        }

        private static void WriteLineHtml(string data, bool isIdentity)
        {
            if (isIdentity)
            {
                data = string.Format("<b>{0}</b>", data);
            }

            HttpContext.Current.Response.Write(data);
            HttpContext.Current.Response.Write("<br />");
        }

        private static void HeadingHtml(string data)
        {
            HttpContext.Current.Response.Output.Write("<h2>{0}</h2>", data);
        }
    }
}
