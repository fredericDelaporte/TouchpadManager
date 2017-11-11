using System.ComponentModel;

namespace TouchpadManager
{
    /// <summary>
    /// Service installer
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
