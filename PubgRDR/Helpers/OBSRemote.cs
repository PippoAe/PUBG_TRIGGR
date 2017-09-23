using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OBSWebsocketDotNet;
using System.Windows.Forms;
using System.Threading;

namespace PubgTriggr
{
    class OBSRemote
    {
        private static OBSRemote instance;

        private OBSRemote()
        {
        }

        public static OBSRemote Instance
        {
            get
            {
                if (instance == null)
                    instance = new OBSRemote();
                return instance;
            }

        }
        private OBSWebsocket _obs = new OBSWebsocket();

        public String Connect()
        {
            try
            {
                _obs.Connect(Properties.Settings.Default.OBSRemoteAdress, Properties.Settings.Default.OBSRemotePassword);
                return "Connected to OBSRemote-Socket.";
            }
            catch (AuthFailureException)
            {
                return "Authentication failed.";
            }
            catch (ErrorResponseException ex)
            {
                return "Connect failed : " + ex.Message;
            }
        }

        public List<OBSScene> GetScenes()
        {
            return _obs.ListScenes();
        }

        public List<OBSSceneItem> GetSceneItems(string SceneName)
        {
            foreach (OBSScene scene in _obs.ListScenes())
            {
                if(scene.Name == SceneName)
                {
                    return scene.Items;
                }
            }
            return null;
        }
       
        public string TriggerSceneItem(String SceneItemName)
        {
            try {
                //Toggle Scene Item
                _obs.SetSourceRender(SceneItemName, false);
                _obs.SetSourceRender(SceneItemName, true);
                Thread.Sleep(5000);
                _obs.SetSourceRender(SceneItemName, false);

            }
            catch(Exception e)
            {
                return String.Format("Error while triggering scene '{0}' {1}", SceneItemName,e.Message);
            }
            return String.Format("Scene {0} triggered.", SceneItemName);
        }
    }
}
