using System.Collections;
using System.Collections.Generic;
using UnityEngine;



class TimerNode
{
    public Timer.TimerHandler callback;
    public float duration;
    public float delay;//������ʱ�䴥��һ��
    public int times;//�����Ĵ���
    public float passedTime;//Timer��ȥ�˶���
    public object param;//��Ҫʲô���͵�����
    public int timerId;
    public bool isRemoved = false;
}
public class Timer : MonoBehaviour
{
    public static Timer Instance = null;
    public delegate void TimerHandler(object param);
    private Dictionary<int, TimerNode> timers = null;
    private int autoId = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {//��֤����
            GameObject.Destroy(this);
            return;
        }
        this.Init();//��ʼ��
    }
    private void Update()
    {

        float dt = Time.deltaTime;
        foreach (TimerNode timer in this.timers.Values)
        {
            if (timer.isRemoved)
            {
                continue;
            }
            timer.passedTime += dt;
            if (timer.passedTime >= (timer.delay + timer.duration))
            {
                timer.callback(timer.param); ;
                timer.times--;
                timer.passedTime -= (timer.delay + timer.duration);
                timer.delay = 0;
            }
            if (timer.times == 0)
            {
                timer.isRemoved = true;
                //ɾ��
            }
        }
    }
    private void Init()
    {
        this.timers = new Dictionary<int, TimerNode>();
    }
    //����timer
    public int ScheduleOnce(TimerHandler func, float delay)
    {
        return Schedule(func, 1, 0, delay);
    }
    public int ScheduleOnce(TimerHandler func, object param, float delay)
    {

        return Schedule(func, param, 1, 0, delay);
    }
    public int Schedule(TimerHandler fuc, int times, float delay, float duration)
    {
        return Schedule(fuc, null, times, delay, duration);
    }
    public int Schedule(TimerHandler func, object param, int times, float delay, float duration)
    {
        TimerNode timer = new TimerNode();
        timer.callback = func;
        timer.duration = duration;
        timer.param = param;
        timer.times = times;
        timer.delay = delay;
        timer.passedTime = duration;
        timer.timerId = this.autoId;
        timer.isRemoved = false;
        autoId++;
        this.timers.Add(timer.timerId, timer);
        return timer.timerId;
    }



}


