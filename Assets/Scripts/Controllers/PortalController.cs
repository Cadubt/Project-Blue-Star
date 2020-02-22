using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class PortalController : BaseController
    {
        public string SceneToTeleport;
        public Vector3 position;


        //Passa de fase
        private void OnTriggerEnter(Collider other)
        {
            BaseController cont = new BaseController();

            if (other.gameObject.tag == "Player")
            {
                cont.positionOnMap = transform.position;
                //DontDestroyOnLoad(this);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                SceneManager.LoadScene(SceneToTeleport);
            }
            
            

        }
    }
}
