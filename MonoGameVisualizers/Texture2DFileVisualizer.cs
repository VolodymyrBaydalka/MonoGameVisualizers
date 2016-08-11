using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.VisualStudio.DebuggerVisualizers;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(MonoGameVisualizers.Texture2DFileDebuggerVisualizer),
    typeof(MonoGameVisualizers.Texture2DFileVisualizerObjectSource),
    Target = typeof(Texture2D), Description = "Texture2D Visualizer")]
namespace MonoGameVisualizers
{
    class Texture2DFileVisualizerObjectSource : VisualizerObjectSource
    {
        #region Implementation
        public override void GetData(object target, Stream outgoingData)
        {
            Texture2D texture = target as Texture2D;
            string fileName = Path.GetTempFileName() + ".png";

            using (var stream = File.Create(fileName))
            {
                texture.SaveAsPng(stream, texture.Width, texture.Height);
            }

            new BinaryWriter(outgoingData).Write(fileName);
        }
        #endregion
    }

    public class Texture2DFileDebuggerVisualizer : DialogDebuggerVisualizer
    {
        #region Imlementation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowService"></param>
        /// <param name="objectProvider"></param>
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            Process.Start(new BinaryReader(objectProvider.GetData()).ReadString());
        }
        #endregion
    }
}
