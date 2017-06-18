using System.Drawing;

namespace WindowsFormsApplication1
{
    public partial class Screensaver
    {
        //Determines the target frames per second
        private int fps = 60;
        // Determines the size of the internal bitmap to render to.
        // 0.5 would be half the size of the actual window.
        private double internalScale = 1.0;

        /// <summary>
        /// Allows you to add fields to the settings form.
        /// You must first set the registry sub key.
        /// I recomend the name of the screensaver.
        /// </summary>
        public static void SetFields()
        {

        }

        /// <summary>
        /// Allows you to set up any needed variables before any rendering is done.
        /// </summary>
        private void Setup()
        {

        }

        /// <summary>
        /// Allows you to update any needed variables before any rendering is done.
        /// Happens roughly at the same speed as fps.
        /// </summary>
        private void Tick()
        {

        }

        /// <summary>
        /// Draws things to an underlying bitmap that will then get rendered to the screen.
        /// </summary>
        /// <param name="g">Graphics object for drawing</param>
        /// <param name="width">Width of the drawing space</param>
        /// <param name="height">Height of the drawing space</param>
        private void Render(Graphics g, int width, int height)
        {

        }

    }
}
