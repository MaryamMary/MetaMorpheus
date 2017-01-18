﻿using System.Text;

namespace InternalLogicEngineLayer
{
    public class ClassicSearchResults : MyResults
    {
        #region Internal Constructors

        internal ClassicSearchResults(ClassicSearchEngine searchParams) : base(searchParams)
        {
        }

        #endregion Internal Constructors

        #region Public Properties

        public ClassicSpectrumMatch[][] OuterPsms { get; set; }

        #endregion Public Properties

        #region Protected Methods

        protected override string StringForOutput
        {
            get
            {
                var sb = new StringBuilder();
                return sb.ToString();
            }
        }

        #endregion Protected Methods
    }
}