using System.Diagnostics;
using System.Text;

namespace Aurora.EndPoints.Procyon.Providers;

public static class ZapretProvider
{
    private static Process _process;
    private static StringBuilder _outputBuffer = new StringBuilder();
    private static bool _isRunning = false;

    /// <summary>
    /// Событие для получения вывода процесса.
    /// </summary>
    public static event Action<string> OnOutputReceived;

    /// <summary>
    /// Запускает консольное приложение.
    /// </summary>
    /// <param name="filePath">Путь к исполняемому файлу.</param>
    public static void Start(string filePath)
    {
        if (_isRunning)
        {
            throw new InvalidOperationException("Процесс уже запущен.");
        }

        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = filePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false
            },
            EnableRaisingEvents = true
        };

        _process.OutputDataReceived += (sender, e) => HandleOutput(e.Data);
        _process.ErrorDataReceived += (sender, e) => HandleOutput(e.Data);

        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        _isRunning = true;
    }

    /// <summary>
    /// Асинхронно ожидает завершения процесса.
    /// </summary>
    public static async Task WaitForExitAsync()
    {
        if (_process == null || !_isRunning)
        {
            throw new InvalidOperationException("Процесс не запущен.");
        }

        await Task.Run(() => _process.WaitForExit());
        _isRunning = false;
    }

    /// <summary>
    /// Принудительно завершает процесс.
    /// </summary>
    public static void Stop()
    {
        if (_process != null && !_process.HasExited)
        {
            _process.Kill();
            _process.WaitForExit();
            _isRunning = false;
        }
    }

    /// <summary>
    /// Возвращает весь собранный вывод процесса.
    /// </summary>
    public static string GetOutput()
    {
        return _outputBuffer.ToString();
    }

    /// <summary>
    /// Обрабатывает вывод процесса.
    /// </summary>
    private static void HandleOutput(string data)
    {
        if (!string.IsNullOrEmpty(data))
        {
            _outputBuffer.AppendLine(data);
            OnOutputReceived?.Invoke(data);
        }
    }

    /// <summary>
    /// Очищает внутренний буфер вывода.
    /// </summary>
    public static void ClearOutput()
    {
        _outputBuffer.Clear();
    }
}