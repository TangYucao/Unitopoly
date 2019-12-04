// This is for selection UI. Data should be retrieved from database or static data like txt or csv.
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class SwitchCarUI : MonoBehaviour
{
    public static SwitchCarUI instance;
    public string car_module_name;
    void Awake()
    {
        instance = this;
    }


    [HideInInspector] public bool selection_made;
    [HideInInspector] public int result_selection;
    public IEnumerator SwitchCar(int player_index)
    {
        selection_made = false;
        result_selection = 0;
        transform.GetChild(0).gameObject.SetActive(true);
        Player.CarCollectionsEnum car_type;
        // Magic logic code.
        bool valid = false;
        Player player = Gameplay.instance.GetPlayers()[player_index];
        while (!selection_made || !valid)
        {
            if (result_selection == 6)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                yield break;
            }
            int bool_index = result_selection - 1;
            // // No choice been made.
            // if (bool_index < 0)
            // {
            //     break;
            // }
            if (bool_index >= 0)
            {
                Debug.Log(player.car_collections_bool[bool_index]);
                if (player.car_collections_bool[bool_index])
                {
                    valid = true;
                }
                else
                {
                    car_type = (Player.CarCollectionsEnum)bool_index;
                    selection_made = false;
                    // yield return MessageAlert.instance.DisplayAlert(player.GetPlayerName() + " switch to " + car_type.ToString() + " failed.", Color.red);
                }
            }
            else
            {
                selection_made = false;
                valid = false;

            }
            yield return null;
        }
                Debug.Log("[61]");

        transform.GetChild(0).gameObject.SetActive(false);
        car_type = (Player.CarCollectionsEnum)result_selection - 1;
        switch (car_type)
        {
            case Player.CarCollectionsEnum.BenzG:
                player.shield = true;
                player.dice_number = 2;
                player.income_ratio = 1;
                break;
            case Player.CarCollectionsEnum.Lamborghini:
                player.shield = false;
                player.dice_number = 3;
                player.income_ratio = 1;
                break;
            case Player.CarCollectionsEnum.Bentley:
                player.shield = false;
                player.dice_number = 2;
                player.income_ratio = 2;
                break;
            case Player.CarCollectionsEnum.Ferrari:
                player.shield = false;
                player.dice_number = 4;
                player.income_ratio = 1;
                break;
            case Player.CarCollectionsEnum.RollsRoyce:
                player.shield = false;
                player.dice_number = 2;
                player.income_ratio = 3;
                break;
        }
        yield return MessageAlert.instance.DisplayAlert(player.GetPlayerName() + " switch to " + car_type.ToString() + ".", Color.green);
        GameObject module = Gameplay.instance.carPrefab.transform.Find(car_module_name).gameObject;
        GameObject new_module = ((GameObject)(Instantiate(module, new Vector3(0, 0, 0), module.transform.rotation)));
        Destroy(player.current_module);
        new_module.transform.parent = player.transform;
        new_module.transform.position = player.transform.position;
        new_module.transform.localEulerAngles =  new Vector3(0, 0,0);
        player.current_module = new_module;


    }

    public void Select1()
    {
        result_selection = 1;
        selection_made = true;
        car_module_name = "BenzG";
    }

    public void Select2()
    {
        result_selection = 2;
        selection_made = true;
        car_module_name = "Lamborghini";

    }

    public void Select3()
    {
        result_selection = 3;
        selection_made = true;
        car_module_name = "Bentley";

    }

    public void Select4()
    {
        result_selection = 4;
        selection_made = true;
        car_module_name = "Ferrari";

    }
    public void Select5()
    {
        result_selection = 5;
        selection_made = true;
        car_module_name = "Phantom";
    }
    public void SelectClose()
    {
        result_selection = 6;
        selection_made = true;
        car_module_name = "Phantom";
    }
}
