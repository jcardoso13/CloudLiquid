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
        /// <summary>
        /// Initializes a new instance of the BaseCloudLiquidTag/> class.
        /// </summary>
        public BaseCloudLiquidTag() { }

        /// <summary>
        /// Initializes a new instance of the BaseCloudLiquidTag class with the specified logger.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public BaseCloudLiquidTag(ILogger logger) 
        { 
            this.InitializeLogger(logger); 
        }

        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets the logger instance.
        /// </summary>
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
        /// <summary>
        /// Initializes the logger instance.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public void InitializeLogger(ILogger logger) { this.logger = logger; }

        /// <summary>
        /// Initializes the tag with the specified tagName, markup, and tokens.
        /// </summary>
        /// <param name="tagName">The name of the tag.</param>
        /// <param name="markup">The markup for the tag.</param>
        /// <param name="tokens">The list of tokens.</param>
        /// <exception>Thrown when the markup syntax is incorrect.</exception>
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