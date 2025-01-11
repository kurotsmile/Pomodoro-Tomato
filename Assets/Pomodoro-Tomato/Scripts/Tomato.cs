using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Tomato_Step_Type {work,sleep,sleep_long}
public class Tomato : MonoBehaviour
{
    [Header("Obj App")]
    public Apps app;
    public Timer_Item[] timer_item;
    public Timer_Item[] timer_item_work;
    public Timer_Item[] timer_item_sleep;
    public Timer_Item[] timer_item_sleep_long;
    public Sprite sp_icon_play;
    public Sprite sp_icon_pause;
    public Sprite sp_icon_check;
    public Animator ani_tomato_staus;
    public Color32 color_done;
    public Color32 color_nomal;

    [Header("UI Emp")]
    public Slider slider_timer_total;
    public Slider slider_timer_cur;
    public Text txt_timer;
    public Text txt_timer_tip;
    public Image img_icon_play;
    public GameObject obj_btn_stop;
    public GameObject obj_btn_skip;

    public float timeRemaining = 10;
    public float timeRemaining_total = 0;
    public bool timerIsRunning = false;

    private int index_timer_item = -1;
    private bool is_check_next_steep = false;

    public void on_load()
    {
        this.check_icon_status_play();
        this.on_reset_all_step_timer();
        this.update_ui_emp();
        this.ani_tomato_staus.Play("tomato_nomal");
        this.txt_timer_tip.text = "Press the play button to start";
        this.is_check_next_steep = false;
        this.on_load_data_all_step();
    }

    public void on_load_data_all_step()
    {
        for (int i = 0; i < this.timer_item.Length; i++) this.timer_item[i].on_load();
    }

    private void on_reset_all_step_timer()
    {
        for (int i = 0; i < this.timer_item.Length; i++) this.timer_item[i].on_nomal(this.color_nomal);
    }

    private float timer_total_all_step()
    {
        float timer_total = 0;
        for (int i = 0; i < this.timer_item.Length; i++)
        {
            timer_total+=this.timer_item[i].timer_count_second;
        }
        return timer_total;
    }

    private void update_ui_emp()
    {
        this.slider_timer_total.maxValue = this.timer_total_all_step();
    }

    public void select_item_timer(int index_item)
    {
        this.timerIsRunning = true;
        this.timeRemaining = this.timer_item[index_item].timer_count_second;
        this.slider_timer_cur.maxValue= this.timer_item[index_item].timer_count_second;
        Timer_Item i_timer = this.timer_item[index_item].GetComponent<Timer_Item>();
        i_timer.on_play();
        this.txt_timer_tip.text = i_timer.txt_tip.text;
        if (i_timer.type == Tomato_Step_Type.work)
            this.ani_tomato_staus.Play("tomato_status");
        else if (i_timer.type == Tomato_Step_Type.sleep)
            this.ani_tomato_staus.Play("tomato_sleep");
        else
            this.ani_tomato_staus.Play("tomato_sleep");
        this.update_ui_emp();
    }

    void Update()
    {
        if (this.timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                float minutes = Mathf.FloorToInt(timeRemaining / 60);
                float seconds = Mathf.FloorToInt(timeRemaining % 60);
                this.txt_timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                this.slider_timer_cur.value = this.timeRemaining;
            }
            else
            {
                this.timerIsRunning = false;
                this.success_timer_step();
            }

            this.timeRemaining_total += Time.deltaTime;
            this.slider_timer_total.value = this.timeRemaining_total;
        }
    }

    private void success_timer_step()
    {
        if(this.get_timer_cur().type==Tomato_Step_Type.work) this.app.add_scores_tomato();

        this.txt_timer_tip.text = "Click the next button to continue the process";
        this.ani_tomato_staus.Play("tomato_success");
        this.app.play_sound(2);
        this.txt_timer.text = "00:00";
        this.img_icon_play.sprite = this.sp_icon_check;
        this.is_check_next_steep = true;
        this.app.carrot.play_vibrate();
    }

    public void next_timer(bool is_skip)
    {
        this.get_timer_cur().on_done(this.color_done);
        if (this.get_timer_cur().type==Tomato_Step_Type.work) this.app.report.add_report_tomato(is_skip);

        this.index_timer_item++;
        if (this.index_timer_item >= this.timer_item.Length)
        {
            this.index_timer_item = 0;
            this.on_reset_all_step_timer();
        }
        this.select_item_timer(this.index_timer_item);
        this.check_icon_status_play();
    }

    private Timer_Item get_timer_cur()
    {
        return this.timer_item[this.index_timer_item];
    }

    public void on_play_or_pause()
    {
        if (this.is_check_next_steep)
        {
            this.next_timer(false);
        }
        else
        {
            if (this.timerIsRunning)
                this.timerIsRunning = false;
            else
            {
                this.app.play_sound(1);
                this.timerIsRunning = true;
                if (this.index_timer_item == -1)
                {
                    this.index_timer_item = 0;
                    this.select_item_timer(this.index_timer_item);
                }
            }
        }

        this.check_icon_status_play();
        this.update_ui_emp();
    }

    private void check_icon_status_play()
    {
        if (this.timerIsRunning)
        {
            this.img_icon_play.sprite = this.sp_icon_pause;
            if(this.get_timer_cur().type==Tomato_Step_Type.work)
                this.ani_tomato_staus.Play("tomato_status");
            else
                this.ani_tomato_staus.Play("tomato_sleep");
        }  
        else
        {
            this.img_icon_play.sprite = this.sp_icon_play;
            this.ani_tomato_staus.Play("tomato_nomal");
        }

        if (this.index_timer_item == -1)
        {
            this.obj_btn_skip.SetActive(false);
            this.obj_btn_stop.SetActive(false);
        }
        else
        {
            this.obj_btn_skip.SetActive(true);
            this.obj_btn_stop.SetActive(true);
        }
            
    }

    public void on_stop()
    {
        this.index_timer_item = -1;
        this.timeRemaining_total = 0;
        this.timerIsRunning = false;
        this.slider_timer_cur.value = this.slider_timer_cur.maxValue;
        this.slider_timer_total.value = 0;
        this.check_icon_status_play();
        this.on_reset_all_step_timer();
        this.ani_tomato_staus.Play("tomato_nomal");
        this.txt_timer_tip.text = "Press the play button to start";
        this.is_check_next_steep = false;
    }

    public float get_timer_work()
    {
        return this.timer_item_work[0].timer_count_minute;
    }

    public float get_timer_sleep()
    {
        return this.timer_item_sleep[0].timer_count_minute;
    }

    public float get_timer_sleep_long()
    {
        return this.timer_item_sleep_long[0].timer_count_minute;
    }

    public void set_timer_work(float new_timer)
    {
        for (int i = 0; i < this.timer_item_work.Length; i++) this.timer_item_work[i].set_timer(new_timer);
    }

    public void set_timer_sleep(float new_timer)
    {
        for (int i = 0; i < this.timer_item_sleep.Length; i++) this.timer_item_sleep[i].set_timer(new_timer);
    }

    public void set_timer_sleep_long(float new_timer)
    {
        this.timer_item_sleep_long[0].set_timer(new_timer);
    }

    public void freshen_timer_cur()
    {
        if(this.index_timer_item!=-1) this.select_item_timer(this.index_timer_item);
    }
}
