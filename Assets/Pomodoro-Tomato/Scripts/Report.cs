using System;
using UnityEngine;
using UnityEngine.UI;

public class Report : MonoBehaviour
{
    [Header("App Obj")]
    public Apps app;
    public GameObject panel_report;
    public Transform tr_all_day;
    public GameObject item_day_prefab;
    public GameObject item_tomato_prefab;
    public Color32 color_nomal;
    public Color32 color_today;

    public Color32 color_tomato_excellent;
    public Color32 color_tomato_bad;
    public Color32 color_tomato_none;
    [Header("Ui")]
    public Text txt_count_tomato;

    private DateTime date_cur;
    private string id_date_cur;
    private int count_tomato_cur = 0;
    private int count_tomato_big = 0;
    public void on_load()
    {
        this.date_cur = DateTime.Now;
        this.id_date_cur = this.date_cur.ToString("dd/MM/yyyy");
        this.count_tomato_cur = PlayerPrefs.GetInt("p_cur_" + this.id_date_cur,0);
        this.count_tomato_big= PlayerPrefs.GetInt("p_big_" + this.id_date_cur, 0);
        this.txt_count_tomato.text = (this.count_tomato_big+1).ToString();
        this.panel_report.SetActive(false);
    }

    public void add_report_tomato(bool is_skip)
    {
        if (is_skip == false)
            PlayerPrefs.SetInt("p_" + this.count_tomato_big + "_" + this.count_tomato_cur + "_" + this.id_date_cur, 0);
        else
            PlayerPrefs.SetInt("p_" + this.count_tomato_big + "_" + this.count_tomato_cur + "_" + this.id_date_cur, 1);

        this.count_tomato_cur++;
        if (this.count_tomato_cur >= 4)
        {
            this.count_tomato_cur = 0;
            this.count_tomato_big++;
            this.txt_count_tomato.text = (this.count_tomato_big + 1).ToString();
            PlayerPrefs.SetInt("p_big_" + this.id_date_cur, this.count_tomato_big);
        }
        PlayerPrefs.SetInt("p_cur_" + this.id_date_cur, this.count_tomato_cur);
    }

    public void show_report()
    {
        this.app.carrot.clear_contain(this.tr_all_day);
        DateTime date_start = this.date_cur.AddDays(-3);
        DateTime date_end = this.date_cur.AddDays(+4);

        TimeSpan interval = new TimeSpan(24, 0, 0);
        int numIntervals = (int)((date_end - date_start).TotalMinutes / interval.TotalMinutes) + 1;

        DateTime[] dateTimeArray = new DateTime[numIntervals];

        for (int i = 0; i < numIntervals; i++)
        {
            dateTimeArray[i] = date_start.Add(interval.Multiply(i));
        }

        foreach (DateTime dateTime in dateTimeArray)
        {
            string id_s_date = dateTime.ToString("dd/MM/yyyy");
            GameObject day_obj = Instantiate(this.item_day_prefab);
            day_obj.transform.SetParent(this.tr_all_day);
            day_obj.transform.position = Vector3.zero;
            day_obj.transform.localScale = new Vector3(1f, 1f, 1f);
            Day_Item d = day_obj.GetComponent<Day_Item>();
            d.txt_title.text = dateTime.ToString("dd/MM");
            if (dateTime == this.date_cur)
                d.img_bk.color = this.color_today;
            else
                d.img_bk.color = this.color_nomal;
            this.app.carrot.clear_contain(d.tr_body);
            int num_big_p;
            if (dateTime == this.date_cur)
                num_big_p = PlayerPrefs.GetInt("p_big_" + id_s_date, 1);
            else
                num_big_p = PlayerPrefs.GetInt("p_big_" + id_s_date, 0);

            for (int i = 0; i < num_big_p; i++)
            {
                GameObject p_obj = Instantiate(this.item_tomato_prefab);
                p_obj.transform.SetParent(d.tr_body);
                p_obj.transform.position = Vector3.zero;
                p_obj.transform.localScale = new Vector3(1f, 1f, 1f);
                Tomato_Item t = p_obj.GetComponent<Tomato_Item>();
                for (int y = 0; y < t.img_tomato.Length; y++)
                {
                    int type_p = PlayerPrefs.GetInt("p_" + i + "_" + y + "_" + id_s_date, -1);
                    if (type_p == 0)
                        t.img_tomato[y].color = this.color_tomato_excellent;
                    else if (type_p == 1)
                        t.img_tomato[y].color = this.color_tomato_bad;
                    else
                        t.img_tomato[y].color = this.color_tomato_none;
                }
            }

        }

        this.panel_report.SetActive(true);
    }

    public void close()
    {
        this.GetComponent<Apps>().carrot.play_sound_click();
        this.panel_report.SetActive(false);
    }
}
