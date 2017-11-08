using System.ServiceProcess;

namespace TouchpadManager
{
    /// <summary>
    /// The touchpad service class.
    /// </summary>
    public partial class TouchpadService : ServiceBase
    {
        private readonly TouchpadHandler _handler = new TouchpadHandler();
        
        /// <inheritdoc />
        public TouchpadService()
        {
            InitializeComponent();
        }

        /// <inheritdoc />
        protected override void OnStart(string[] args)
        {
            _handler.Start();
        }

        /// <inheritdoc />
        protected override void OnStop()
        {
            _handler.Stop();
        }
    }
}
