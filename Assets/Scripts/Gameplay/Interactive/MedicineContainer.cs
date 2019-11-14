namespace uqac.timesick.gameplay
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;


    public class MedicineContainer : MonoBehaviour, Interactive
    {
        // True if the medicine has not yet been stolen, false otherwise
        private bool hasMedicine = true;

        private SpriteRenderer rd;


        void Start()
        {
            rd = GetComponent<SpriteRenderer>();
        }

        void Update()
        {


        }
        public void Select()
        {
            //Visual feedback, if the mainCharacter can interact wiht this object
            rd.material.SetInt("_OutlineEnabled", 1);
            rd.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
        }

        public void Deselect()
        {
            //Remove the Visual feedback, if the mainCharacter cannot interact wiht this object any longer
            rd.material.SetInt("_OutlineEnabled", 0);
            rd.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        public UserAction GetAction(MainCharacter mainCharacter)
        {
            if (MedicineAvailable())
            {
                return new UserAction("Voler", Button.A, new List<Button>() { Button.UP, Button.DOWN }, 3, () => mainCharacter.CollectMedicine(this));
            }
            else
            {
                return null;
            }
        }

        public bool StealMedicine()
        {
            if (MedicineAvailable())
            {
                hasMedicine = false;
                return true;
            }
            else
            {
                return false;
            }
                
        }


        private bool MedicineAvailable()
        {
            return hasMedicine;
        }

    }
}