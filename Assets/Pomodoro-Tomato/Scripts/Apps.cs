using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Apps : MonoBehaviour
{
    [Header("App Obj")]
    public Carrot.Carrot carrot;
    public Tomato tomato;
    public Report report;

    public IronSourceAds ads;

    [Header("Ui")]
    public Text txt_score_tomato;

    [Header("Sounds")]
    public AudioSource[] sound;

    [Header("Setting")]
    public Sprite sp_icon_work;
    public Sprite sp_icon_sleep;
    public Sprite sp_icon_sleep_long;
    public Sprite sp_icon_report;

    private int count_tomato = 0;
    private Carrot.Carrot_Window_Input inp_timer;
    private Carrot.Carrot_Box_Item item_change_timer;
    private Carrot.Carrot_Box box_setting;

    async void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        await this.carrot.Load_CarrotAsync(this.check_exit_app);
        this.ads.On_Load();
        this.carrot.act_after_delete_all_data = this.act_delete_all_data;
        this.carrot.game.load_bk_music(this.sound[0]);
        this.tomato.on_load();
        this.report.on_load();

        this.count_tomato = PlayerPrefs.GetInt("count_tomato", 0);
        this.update_ui_emp();
    }

    private void check_exit_app()
    {
        if (this.report.panel_report.activeInHierarchy)
        {
            this.report.close();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void btn_play()
    {
        this.ads.show_ads_Interstitial();
        this.carrot.play_sound_click();
        this.tomato.on_play_or_pause();
    }

    public void btn_stop()
    {
        this.ads.show_ads_Interstitial();
        this.play_sound(3);
        this.tomato.on_stop();
    }

    public void btn_setting()
    {
        this.ads.show_ads_Interstitial();
        this.ads.HideBannerAd();
        if (this.box_setting != null) this.box_setting.close();
        this.box_setting=this.carrot.Create_Setting();

        Carrot.Carrot_Box_Item item_timer_report = box_setting.create_item_of_top("i_report");
        item_timer_report.set_icon(this.sp_icon_report);
        item_timer_report.set_title("View Statistics");
        item_timer_report.set_tip("Statistics for your working week");
        item_timer_report.set_act(() => this.btn_report());

        Carrot.Carrot_Box_Item item_timer_sleep_long = box_setting.create_item_of_top("i_sleep_long");
        item_timer_sleep_long.set_icon(this.sp_icon_sleep_long);
        item_timer_sleep_long.set_title("Set a long break time ("+this.tomato.get_timer_sleep_long()+ " Minute)");
        item_timer_sleep_long.set_tip("change the length of the long break");
        item_timer_sleep_long.set_act(() => this.show_change_timer(item_timer_sleep_long));

        Carrot.Carrot_Box_Item item_timer_sleep = box_setting.create_item_of_top("i_sleep");
        item_timer_sleep.set_icon(this.sp_icon_sleep);
        item_timer_sleep.set_title("Set a short break time (" + this.tomato.get_timer_sleep() + " Minute)");
        item_timer_sleep.set_tip("change the length of the short break");
        item_timer_sleep.set_act(() => this.show_change_timer(item_timer_sleep));

        Carrot.Carrot_Box_Item item_timer_work = box_setting.create_item_of_top("i_work");
        item_timer_work.set_icon(this.sp_icon_work);
        item_timer_work.set_title("Set working time (" + this.tomato.get_timer_work() + " Minute)");
        item_timer_work.set_tip("Change working hours");
        item_timer_work.set_act(() => this.show_change_timer(item_timer_work));
        this.box_setting.set_act_before_closing(this.act_close_setting);
    }

    private void act_delete_all_data()
    {
        this.carrot.delay_function(1f, this.Start);
    }

    private void act_close_setting()
    {
        this.ads.ShowBannerAd();
    }

    public void show_change_timer(Carrot.Carrot_Box_Item item_change)
    {
        this.item_change_timer = item_change;
        string s_title;
        float val;
        if (item_change.name == "i_work")
        {
            s_title = "Set working time";
            val = this.tomato.get_timer_work();
        }
        else if (item_change.name == "i_sleep")
        {
            s_title = "Set a short break time";
            val = this.tomato.get_timer_sleep();
        }
        else
        {
            s_title = "Set a long break time";
            val = this.tomato.get_timer_sleep_long();
        } 

        this.inp_timer=this.carrot.Show_input(s_title, "Enter the number of minutes of time you want to change");
        this.inp_timer.set_icon(item_change.img_icon.sprite);
        inp_timer.inp_text.contentType = InputField.ContentType.IntegerNumber;
        inp_timer.inp_text.text = val.ToString();
        inp_timer.set_act_done(act_done_change_timer);
    }

    private void act_done_change_timer(string val_timer)
    {
        if (int.Parse(val_timer) > 0)
        {
            if (this.inp_timer != null) this.inp_timer.close();
            if (this.item_change_timer.name == "i_work")
            {
                this.tomato.set_timer_work(float.Parse(val_timer));
            }
            else if (this.item_change_timer.name == "i_sleep")
            {
                this.tomato.set_timer_sleep(float.Parse(val_timer));
            }
            else
            {
                this.tomato.set_timer_sleep_long(float.Parse(val_timer));
            }

            Debug.Log("change done:" + val_timer + " id name:" + this.item_change_timer.name);
            this.tomato.on_load_data_all_step();
            this.tomato.freshen_timer_cur();
            this.btn_setting();
        }
        else
        {
            this.carrot.Show_msg("Change time", "the number of minutes entered must be greater than 0",Carrot.Msg_Icon.Error);
        }

    }

    public void btn_skip()
    {
        this.carrot.play_sound_click();
        this.tomato.next_timer(true);
    }

    public async void btn_user()
    {
        await this.carrot.user.Show_loginAsync();
    }

    public void btn_rate()
    {
        this.carrot.show_rate();
    }

    public void btn_share()
    {
        this.carrot.show_share();
    }

    public async void btn_rank()
    {
        await this.carrot.game.Show_List_Top_player();
    }

    public void play_sound(int index)
    {
        if (this.carrot.get_status_sound()) this.sound[index].Play();
    }

    private void update_ui_emp()
    {
        this.txt_score_tomato.text = this.count_tomato.ToString();
    }

    public async void add_scores_tomato()
    {
        this.count_tomato++;
        await this.carrot.game.update_scores_playerAsync(this.count_tomato);
        PlayerPrefs.SetInt("count_tomato", this.count_tomato);
        this.update_ui_emp();
    }

    public void btn_report()
    {
        this.ads.show_ads_Interstitial();
        if (this.box_setting != null) this.box_setting.close();
        this.carrot.play_sound_click();
        this.report.show_report();
    }
}
