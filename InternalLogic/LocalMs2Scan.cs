﻿using Chemistry;
using MassSpectrometry;
using Spectra;
using System;

namespace InternalLogicEngineLayer
{
    internal class LocalMs2Scan : IComparable, IComparable<double>
    {

        #region Internal Constructors

        internal LocalMs2Scan(IMsDataScan<IMzSpectrum<MzPeak>> b)
        {
            this.theScan = b;
            double monoisotopicPrecursorMZ;
            b.TryGetSelectedIonGuessMonoisotopicMZ(out monoisotopicPrecursorMZ);
            this.monoisotopicPrecursorMZ = monoisotopicPrecursorMZ;

            int monoisotopicPrecursorCharge;
            b.TryGetSelectedIonGuessChargeStateGuess(out monoisotopicPrecursorCharge);
            this.monoisotopicPrecursorCharge = monoisotopicPrecursorCharge;

            precursorMass = monoisotopicPrecursorMZ.ToMass(monoisotopicPrecursorCharge);

            OneBasedScanNumber = b.OneBasedScanNumber;

            RetentionTime = b.RetentionTime;

            NumPeaks = b.MassSpectrum.Count;

            double monoisotopicPrecursorIntensity;
            b.TryGetSelectedIonGuessMonoisotopicIntensity(out monoisotopicPrecursorIntensity);
            this.monoisotopicPrecursorIntensity = monoisotopicPrecursorIntensity;

            TotalIonCurrent = b.TotalIonCurrent;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IMsDataScan<IMzSpectrum<MzPeak>> theScan { get; private set; }
        internal double precursorMass { get; private set; }
        internal double monoisotopicPrecursorMZ { get; private set; }
        internal int OneBasedScanNumber { get; private set; }
        internal double RetentionTime { get; private set; }
        internal int monoisotopicPrecursorCharge { get; private set; }
        internal int NumPeaks { get; private set; }
        internal double monoisotopicPrecursorIntensity { get; private set; }
        internal double TotalIonCurrent { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public int CompareTo(double other)
        {
            return precursorMass.CompareTo(other);
        }

        public int CompareTo(object obj)
        {
            var other = obj as LocalMs2Scan;
            if (other != null)
                return precursorMass.CompareTo(other.precursorMass);
            return precursorMass.CompareTo((double)obj);
        }

        #endregion Public Methods

    }
}