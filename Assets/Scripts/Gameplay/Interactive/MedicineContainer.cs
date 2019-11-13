namespace uqac.timesick.gameplay
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;


    public class MedicineContainer : MonoBehaviour, Interactive
    {
        // True if the medicine has not yet been stolen, false otherwise
        private bool hasMedicine = true;

        private Renderer renderer;


        void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {


        }
        public void Select()
        {
            //Visual feedback, if the mainCharacter can interact wiht this object
            renderer.material.SetInt("_OutlineEnabled", 1);
            renderer.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
        }

        public void Deselect()
        {
            //Remove the Visual feedback, if the mainCharacter cannot interact wiht this object any longer
            renderer.material.SetInt("_OutlineEnabled", 0);
            renderer.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        public UserAction GetAction(MainCharacter mainCharacter)
        {
            if (MedicineAvailable())
            {
                return new UserAction("Voler", Button.A, new List<Button>() { Button.UP, Button.DOWN }, 3, () => mainCharacter.CollectMedicine());
            }
            else
            {
                return null;
            }
        }

        private bool MedicineAvailable()
        {
            return hasMedicine;
        }

    }
}