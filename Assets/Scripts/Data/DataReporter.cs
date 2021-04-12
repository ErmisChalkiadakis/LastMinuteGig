using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;
using System.Net.Http;
using System;
using System.Linq;

public class DataReporter : MonoBehaviour {
    public const string APP_ID = "LastMinuteGig";
    private const string REMOTE_IP = "127.0.0.1";
    private const string BUFFER_FILE = "event_buffer";

    public string username;
    private List<string> eventBuffer;

    public void Awake() {
        this.eventBuffer = new List<string>();
    }

    /**
     * Start a timer to flush the buffer every couple of seconds.
     */
    public void OnEnable() {
        // Flush the buffer every 10s.
        InvokeRepeating("flushBufferAsync", 0.0f, 10.0f);
    }

    /**
     * Stop the timer for flushing the buffer.
     */
    public void OnDisable() {
        CancelInvoke("flushBufferAsync");
    }

    /**
     * When destoryed, block the thread to flush the buffer.
     */
    public void OnDestroy() {
        this.flushBufferAsync();
    }

    /**
     * Adds an event to the end of the event buffer.
     * These events will be processed later.
     */
    public void OnEvent<T>(T @event) {
        lock (this.eventBuffer) {
            int timeInMilliseconds = Mathf.RoundToInt(Time.timeSinceLevelLoad * 1000);
            string logLine = $"{timeInMilliseconds};{@event}";
            this.eventBuffer.Add(logLine);
        }
    }

    /**
     * Asynchronous call that attempts to flush all recorded event data.
     * This will lock itself, and fail with a warning if the log could not be acquired.
     */
    private void flushBufferAsync() {
        // No work for an empty buffer.
        if (this.eventBuffer.Count <= 0) { return; }

        // See if we can acquire the lock.
        bool lockAcquired = false;
        Monitor.TryEnter(this, ref lockAcquired);
        if (lockAcquired) {
            Task.Run(async () => {
                await FlushBufferThread(this.username, this.eventBuffer);
                Monitor.Exit(this);
            });
        } else {
            Debug.LogWarning("[DataReporter] Attempting to flush the buffer while the buffer was still being flushed.");
        }
    }

    /**
     * Flushes the event buffer.
     * The buffer is flushed to an HTTP backend.
     * Failing to flush to the HTTP backend, the data will be flushed to a file.
     */
    private static async Task FlushBufferThread(string username, List<string> buffer) {
        string bufferLines = "sessiontime(ms);event";

        // Read lines from bufferfile first.
        DataReporter.ensureBufferFile(BUFFER_FILE);
        string[] fileLines = System.IO.File.ReadAllLines(BUFFER_FILE);
        bufferLines += "\n" + String.Join("\n", fileLines.Skip(1));

        // Add all lines from the in-memory buffer.
        lock (buffer) {
            foreach (string line in buffer) { bufferLines += "\n" + line; }
            buffer.Clear();
        }

        // Overwrite the buffer file with it's new contents.
        System.IO.File.WriteAllText(BUFFER_FILE, bufferLines);

        try {
            // Try to upload data to server.
            string url = string.Format("http://{0}/?app={1}&username={2}", DataReporter.REMOTE_IP, DataReporter.APP_ID, username);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url, new ByteArrayContent(Encoding.ASCII.GetBytes(bufferLines)));
            response.EnsureSuccessStatusCode();

            // Clear the buffer file.
            System.IO.File.WriteAllText(BUFFER_FILE, "sessiontime(ms);event");
        } catch (Exception e) {
            // Ignore all exceptions here, just log them.
            Debug.LogError("[DataReporter] Encountered an exception while attempting to upload the data. (" + e.Message + ")");
        }
    }

    /**
     * Will try to create the buffer file if it does not exist yet.
     */
    private static void ensureBufferFile(string filename) {
        if (!System.IO.File.Exists(filename)) {
            System.IO.File.AppendAllText(filename, "sessiontime(ms);event");
        }
    }
}
