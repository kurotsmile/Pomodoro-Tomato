using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer_Item : MonoBehaviour
{
    public string id_name;
    public float timer_count_second;
    public float timer_count_minute;
    public Animator ani;
    public Tomato_Step_Type type;
    public Text txt_title;
    public Text txt_tip;
    public Image img_bk;
    public Image img_icon;

    private UnityAction act;

    public void on_load()
    {
        this.timer_count_minute = PlayerPrefs.GetFloat("minute_"+id_name,this.timer_count_minute);
        this.timer_count_second = this.timer_count_minute * 60;
        this.txt_title.text = this.timer_count_minute.ToString()+"m";
    }

    public void on_nomal(Color color_n)
    {
        this.ani.enabled = false;
        this.ani.Play(this.id_name + "_nomal");
        this.img_icon.color = Color.white;
        this.img_bk.color = color_n;
        this.txt_title.color = Color.yellow;
        this.txt_tip.color = Color.white;
    }

    public void on_play()
    {
        this.ani.enabled = true;
        this.ani.Play(this.id_name + "_play");
        this.img_icon.color = Color.black;
        this.txt_title.color = Color.black;
        this.txt_tip.color = Color.black;
    }

    public void on_done(Color32 c_done)
    {
        this.ani.enabled = false;
        this.img_bk.color = c_done;
        this.img_icon.color = Color.black;
        this.txt_title.color = Color.black;
        this.txt_tip.color = new Color(77, 0, 0, 255);
    }

    public void click()
    {
        if(this.act!=null) this.act();
    }

    public void set_act(UnityAction act_set)
    {
        this.act = act_set;
    }

    public void set_timer(float new_timer_minute)
    {
        this.timer_count_minute = new_timer_minute;
        PlayerPrefs.SetFloat("minute_" + id_name, this.timer_count_minute);
        this.timer_count_second = (this.timer_count_minute * 60);
    }
}
