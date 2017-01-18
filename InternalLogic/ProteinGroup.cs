﻿using OldInternalLogic;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternalLogicEngineLayer
{
    public class ProteinGroup
    {

        #region Public Fields

        public readonly bool isDecoy;
        public readonly double proteinGroupScore;

        #endregion Public Fields

        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        internal ProteinGroup(HashSet<Protein> proteins, List<NewPsmWithFdr> psmList, HashSet<CompactPeptide> allUniquePeptides, List<MorpheusModification> variableModifications, List<MorpheusModification> localizeableModifications)
        {
            this.Proteins = proteins;
            this.PsmList = psmList;
            PeptideList = new List<CompactPeptide>();
            UniquePeptideList = new List<CompactPeptide>();
            proteinGroupScore = 0;
            QValue = 0;

            // if any of the proteins in the protein group are decoys, the protein group is a decoy
            foreach (var protein in proteins)
            {
                if (protein.IsDecoy)
                    isDecoy = true;
            }

            // build list of compact peptides associated with the protein group
            foreach (var psm in psmList)
            {
                CompactPeptide peptide = psm.thisPSM.newPsm.GetCompactPeptide(variableModifications, localizeableModifications);
                PeptideList.Add(peptide);

                // calculate the protein group score
                if (allUniquePeptides.Contains(peptide))
                {
                    UniquePeptideList.Add(peptide);
                    proteinGroupScore += psm.thisPSM.Score;
                }
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public readonly HashSet<Protein> proteins;
        public readonly List<NewPsmWithFDR> psmList;
        public readonly List<CompactPeptide> peptideList;
        public readonly List<CompactPeptide> uniquePeptideList;
        public double QValue { get; set; }
        public int cumulativeTarget { get; set; }
        public int cumulativeDecoy { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static string TabSeparatedHeader
        {
            get
            {
            var sb = new StringBuilder();
            sb.Append("Proteins in group" + '\t');
            sb.Append("Number of proteins in group" + '\t');
            sb.Append("Unique peptides" + '\t');
            sb.Append("Shared peptides" + '\t');
            sb.Append("Number of unique peptides" + '\t');
            sb.Append("Summed MetaMorpheus Score" + '\t');
            sb.Append("Decoy?" + '\t');
            sb.Append("Cumulative Target" + '\t');
            sb.Append("Cumulative Decoy" + '\t');
            sb.Append("Q-Value (%)");
            return sb.ToString();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            // list of proteins in the group
            foreach (Protein protein in Proteins)
                sb.Append("" + protein.Name + " ;; ");
            sb.Append("\t");

            // number of proteins in group
            sb.Append("" + Proteins.Count());
            sb.Append("\t");

            // list of unique peptides
            foreach (CompactPeptide peptide in UniquePeptideList)
            {
                string peptideBaseSequence = string.Join("", peptide.BaseSequence.Select(b => char.ConvertFromUtf32(b)));

                sb.Append("" + peptideBaseSequence + " ;; ");
            }
            sb.Append("\t");

            // list of shared peptides
            foreach (CompactPeptide peptide in PeptideList)
            {
                if (!UniquePeptideList.Contains(peptide))
                {
                    string peptideBaseSequence = string.Join("", peptide.BaseSequence.Select(b => char.ConvertFromUtf32(b)));

                    sb.Append("" + peptideBaseSequence + " ;; ");
                }
            }
            sb.Append("\t");

            // number of unique peptides
            sb.Append("" + UniquePeptideList.Count());
            sb.Append("\t");

            // summed metamorpheus score
            sb.Append(proteinGroupScore);
            sb.Append("\t");

            // isDecoy
            sb.Append(isDecoy);
            sb.Append("\t");

            return sb.ToString();

            /*
            // proteins in protein group
            foreach (Protein protein in Proteins)
                sb.Append("" + protein.FullDescription + " ;; ");
            sb.Append("\t");

            // sequences of proteins in group
            foreach (Protein protein in Proteins)
                sb.Append("" + protein.BaseSequence + " ;; ");
            sb.Append("\t");

            // length of each protein
            foreach (Protein protein in Proteins)
                sb.Append("" + protein.BaseSequence.Length + " ;; ");
            sb.Append("\t");

            // number of proteins in group
            sb.Append("" + Proteins.Count);
            sb.Append("\t");

            // number of psm's for the group
            sb.Append("" + PsmList.Count);
            sb.Append("\t");

            // number of unique peptides
            sb.Append("" + UniquePeptideList.Count());
            sb.Append("\t");

            // summed psm precursor intensity
            sb.Append(summedIntensity);
            sb.Append("\t");

            // summed unique peptide precursor intensity
            sb.Append(summedUniquePeptideIntensity);
            sb.Append("\t");

            // cumulative decoy
            sb.Append(cumulativeDecoy);
            sb.Append("\t");

            // q value
            sb.Append(QValue * 100);
            sb.Append("\t");

            return sb.ToString();
            */
        }

        #endregion Public Methods

    }
}