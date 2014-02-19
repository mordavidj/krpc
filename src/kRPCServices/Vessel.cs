﻿using System;
using System.Collections.Generic;
using KRPC.Service;
using KRPC.Schema.Vessel;
using KSP;

namespace KRPCServices
{
    [KRPCService]
    public class Vessel
    {
        private static double GetResourceAmount (string name, ICollection<PartResource> resources) {
            double amount = 0;
            foreach (var resource in resources) {
                if (resource.resourceName == name) {
                    amount += resource.amount;
                }
            }
            return amount;
        }

        [KRPCProcedure]
        public static Resources GetResources () {
            // Get all resources
            List<PartResource> resources = new List<PartResource> ();
            var vessel = FlightGlobals.ActiveVessel;
            foreach (Part part in vessel.Parts) {
                foreach (PartResource resource in part.Resources) {
                    resources.Add (resource);
                }
            }

            return Resources.CreateBuilder ()
                .SetLiquidFuel (GetResourceAmount("LiquidFuel", resources))
                .SetOxidizer (GetResourceAmount("Oxidizer", resources))
                .SetSolidFuel (GetResourceAmount("SolidFuel", resources))
                .SetMonoPropellant (GetResourceAmount("MonoPropellant", resources))
                .SetElectricCharge (GetResourceAmount("ElectricCharge", resources))
                .Build ();
        }
    }
}
