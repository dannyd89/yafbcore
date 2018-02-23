using FlattiBase.Interfaces;
using FlattiBase.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Screens
{
    public abstract class Screen
    {
        private string screenName;

        public string ScreenName
        {
            get 
            { 
                return screenName; 
            }

            protected set
            {
                screenName = value;
            }
        }

        protected ScreenManager parent;

        public ScreenManager Parent
        {
            get
            {
                return parent;
            }

            protected set
            {
                parent = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsUpdatable
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsDrawable
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenName"></param>
        internal Screen(ScreenManager parent, string screenName)
        {
            this.parent = parent;
            this.screenName = screenName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyData"></param>
        public abstract void KeyPressed(System.Windows.Forms.Keys keyData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastUpdate">Time since the last update</param>
        public abstract void Update(TimeSpan lastUpdate);

        /// <summary>
        /// 
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// 
        /// </summary>
        public abstract void Dispose();
    }
}
