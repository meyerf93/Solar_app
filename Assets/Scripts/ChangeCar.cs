using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCar : MonoBehaviour {

    public Text numbers_of_car;
    public int default_num_car;
    public Text total_energy;
    public double default_total_energy;

    public Text State_of_car;
    public Text Production;
    public Text Consumption;

    public Slider slider_prod;
    public Slider slider_conso;

    private string wall = "Plugged on wallbox";
    private string locked = "Plug locked";
    private string ev = "Plugged on EV";

    private bool wall_state = true;
    private bool locked_state = true;
    private bool ev_state = true;

    private int car_old_value;
    private double energy_old_value;

    private void Start()
    {
        numbers_of_car.text = default_num_car.ToString();
        total_energy.text = default_total_energy.ToString();
        car_old_value = default_num_car;
        energy_old_value = default_total_energy;
    }

    public void incresse_car_numbers()
    {
        car_old_value += 1;
        numbers_of_car.text = car_old_value.ToString();
    }

    public void change_total_consumptions()
    {
        energy_old_value += 1.5;
        total_energy.text = energy_old_value.ToString();
    }

    public void change_state_text()
    {
        if (wall_state && locked_state && ev_state)
        {
            State_of_car.text = wall + " " + locked + " " + ev;
            ev_state = false;
        }
        else if (wall_state && locked_state)
        {
            State_of_car.text = wall + " " + locked;
            locked_state = false;
        }
       else if (wall_state)
        {
            State_of_car.text = wall;
            wall_state = true;
            locked_state = true;
            ev_state = true;
        }
    }

    public void change_prod()
    {
        
        slider_conso.value += 10;
        slider_prod.value += 10;

        if(slider_conso.value >= slider_conso.maxValue)
        {
            slider_conso.value = 98;
        }
        if(slider_prod.value >= slider_prod.maxValue)
        {
            slider_prod.value = 205;
        }

        Production.text = slider_prod.value.ToString();
        Consumption.text = slider_conso.value.ToString();        
    }

}
