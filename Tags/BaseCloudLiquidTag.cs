using DotLiquid.Exceptions;
using DotLiquid.Util;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace CloudLiquid
{
    // Base implementation of DotLiquid Tags

    public abstract class BaseCloudLiquidTag : DotLiquid.Block
    {
        #region Private Members

        private ILogger logger;

        private static Regex VariableSegmentRegex => LazyVariableSegmentRegex.Value;
        private static readonly Lazy<Regex> LazyVariableSegmentRegex = new(() => R.B(R.Q(@"\A\s*(?<Variable>{0}+)\s*\Z"), DotLiquid.Liquid.VariableSegment), LazyThreadSafetyMode.ExecutionAndPublication);
        private string to;

        #endregion

        #region Constructors

        public BaseCloudLiquidTag() { }

        public BaseCloudLiquidTag(ILogger logger) 
        { 
            this.InitializeLogger(logger); 
        }

        #endregion

        #region Protected Properties

        protected ILogger Logger 
        { 
            get 
            { 
                return this.logger; 
            } 
        }

        protected string To
        {
            get { return this.to; }
        }

        #endregion

        #region Public Methods

        public void InitializeLogger(ILogger logger) { this.logger = logger; }

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            Match syntaxMatch = VariableSegmentRegex.Match(markup);

            if (syntaxMatch.Success)
            {
                to = syntaxMatch.Groups["Variable"].Value;
            }
            else
            {
                throw new SyntaxException("SyntaxException");
            }

            base.Initialize(tagName, markup, tokens);
        }

        #endregion
    }
}