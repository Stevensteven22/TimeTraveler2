using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Services;

public class GameTBSService : IGameTBSService
{
    public EventHandler<EventArgs> OnConsumed { get; set; }

    public EventHandler<EventArgs> OnProduced { get; set; }

    // 任务队列
    private Queue<object> _tasks = new Queue<object>();

    // 为保证线程安全，使用一个锁来保护_task的访问
    private readonly object _locker = new object();

    // 通过 _wh 给工作线程发信号
    private EventWaitHandle _wh = new AutoResetEvent(false);

    private Thread _worker;

    public GameTBSService()
    {
        // 任务开始，启动工作线程
        _worker = new Thread(Work) { IsBackground = true };
        _worker.Start();
    }

    /// <summary>执行工作</summary>
    private void Work()
    {
        while (true)
        {
            object work = null;
            lock (_locker)
            {
                if (_tasks.Count > 0)
                {
                    work = _tasks.Dequeue(); // 有任务时，出列任务

                    if (work == null) // 退出机制：当遇见一个null任务时，代表任务结束
                        return;
                }
            }

            if (work != null)
                OnConsumed?.Invoke(null, EventArgs.Empty); // 任务不为null时，处理并保存数据
            else
                _wh.WaitOne(); // 没有任务了，等待信号
        }
    }

    // 生产者将数据插入队里中，并给工作线程发信号
    public void Produce(object data)
    {
        EnqueueTask(data);
        OnProduced?.Invoke(null, EventArgs.Empty);
    }

    /// <summary>插入任务</summary>
    private void EnqueueTask(object task)
    {
        lock (_locker)
            _tasks.Enqueue(task); // 向队列中插入任务

        _wh.Set(); // 给工作线程发信号
    }

    // 任务结束

    /// <summary>结束释放</summary>
    public void Dispose()
    {
        EnqueueTask(null); // 插入一个Null任务，通知工作线程退出
        _worker.Join(); // 等待工作线程完成
        _wh.Close(); // 释放资源
    }
}
