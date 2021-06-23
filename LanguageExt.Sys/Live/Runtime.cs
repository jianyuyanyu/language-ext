using System;
using System.Text;
using System.Threading;
using LanguageExt.Effects.Traits;
using LanguageExt.Sys.Traits;
using static LanguageExt.Prelude;

namespace LanguageExt.Sys.Live
{
    public class RuntimeEnv
    {
        public readonly CancellationTokenSource Source;
        public readonly CancellationToken Token;
        public readonly Encoding Encoding;

        public RuntimeEnv(CancellationTokenSource source, CancellationToken token, Encoding encoding)
        {
            Source   = source;
            Token    = token;
            Encoding = encoding;
        }

        public RuntimeEnv(CancellationTokenSource source, Encoding encoding) : this(source, source.Token, encoding)
        {
        }
    }

    /// <summary>
    /// Live IO runtime
    /// </summary>
    public readonly struct Runtime : 
        HasCancel<Runtime>,
        HasConsole<Runtime>,
        HasFile<Runtime>,
        HasEncoding<Runtime>,
        HasTextRead<Runtime>,
        HasTime<Runtime>
    {
        readonly RuntimeEnv Env;

        /// <summary>
        /// Constructor
        /// </summary>
        Runtime(RuntimeEnv env) =>
            Env = env;

        /// <summary>
        /// Constructor function
        /// </summary>
        public static Runtime New() =>
            new Runtime(new RuntimeEnv(new CancellationTokenSource(), System.Text.Encoding.Default));

        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="source">Cancellation token source</param>
        public static Runtime New(CancellationTokenSource source) =>
            new Runtime(new RuntimeEnv(source, System.Text.Encoding.Default));

        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="encoding">Text encoding</param>
        public static Runtime New(Encoding encoding) =>
            new Runtime(new RuntimeEnv(new CancellationTokenSource(), encoding));

        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="encoding">Text encoding</param>
        /// <param name="source">Cancellation token source</param>
        public static Runtime New(Encoding encoding, CancellationTokenSource source) =>
            new Runtime(new RuntimeEnv(source, encoding));

        /// <summary>
        /// Create a new Runtime with a fresh cancellation token
        /// </summary>
        /// <remarks>Used by localCancel to create new cancellation context for its sub-environment</remarks>
        /// <returns>New runtime</returns>
        public Runtime LocalCancel =>
            new Runtime(new RuntimeEnv(new CancellationTokenSource(), Env.Encoding));

        /// <summary>
        /// Direct access to cancellation token
        /// </summary>
        public CancellationToken CancellationToken =>
            Env.Token;

        /// <summary>
        /// Directly access the cancellation token source
        /// </summary>
        /// <returns>CancellationTokenSource</returns>
        public CancellationTokenSource CancellationTokenSource =>
            Env.Source;

        /// <summary>
        /// Get encoding
        /// </summary>
        /// <returns></returns>
        public Encoding Encoding =>
            Env.Encoding;

        /// <summary>
        /// Access the console environment
        /// </summary>
        /// <returns>Console environment</returns>
        public Eff<Runtime, Traits.ConsoleIO> ConsoleEff =>
            SuccessEff(Sys.Live.ConsoleIO.Default);

        /// <summary>
        /// Access the file environment
        /// </summary>
        /// <returns>File environment</returns>
        public Eff<Runtime, Traits.FileIO> FileEff =>
            SuccessEff(Sys.Live.FileIO.Default);

        /// <summary>
        /// Access the TextReader environment
        /// </summary>
        /// <returns>TextReader environment</returns>
        public Eff<Runtime, Traits.TextReadIO> TextReadEff =>
            SuccessEff(Sys.Live.TextReadIO.Default);

        /// <summary>
        /// Access the time environment
        /// </summary>
        /// <returns>Time environment</returns>
        public Eff<Runtime, Traits.TimeIO> TimeEff  =>
            SuccessEff(Sys.Live.TimeIO.Default);
    }
}