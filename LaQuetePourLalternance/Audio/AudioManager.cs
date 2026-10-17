using System.Collections.Generic;

namespace LaQuetePourLalternance;

public class AudioManager
{
    private FMOD.Studio.System _studio;
    private readonly Dictionary<string, FMOD.Studio.EventInstance> _instances = new();
    private Thread? _updateThread;
    private volatile bool _running;

    public void Init()
    {
        Check(FMOD.Studio.System.create(out _studio));
        Check(_studio.initialize(512, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, IntPtr.Zero));

        LoadBank("Master.bank");
        LoadBank("Master.strings.bank");
        // LoadBank("TonAutreBank.bank");

        // Le jeu se bloque souvent sur Console.ReadLine() : on met à jour FMOD en tâche de fond
        _running = true;
        _updateThread = new Thread(() =>
        {
            while (_running)
            {
                _studio.update();
                Thread.Sleep(10);
            }
        }) { IsBackground = true };
        _updateThread.Start();
    }

    private void LoadBank(string fileName)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Audio", "Banks", fileName);
        Check(_studio.loadBankFile(path, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out _));
    }

    public void StartEvent(string eventPath)
    {
        if (_instances.ContainsKey(eventPath)) return; // déjà lancé

        Check(_studio.getEvent(eventPath, out FMOD.Studio.EventDescription desc));
        Check(desc.createInstance(out FMOD.Studio.EventInstance instance));
        Check(instance.start());
        _instances[eventPath] = instance;
    }

    public void StopEvent(string eventPath, bool fadeOut = true)
    {
        if (!_instances.TryGetValue(eventPath, out var instance)) return;

        instance.stop(fadeOut
            ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT
            : FMOD.Studio.STOP_MODE.IMMEDIATE);
        instance.release(); // libère l'instance une fois arrêtée
        _instances.Remove(eventPath);
    }

    public void Shutdown()
    {
        _running = false;
        _updateThread?.Join();
        _studio.release();
    }

    private static void Check(FMOD.RESULT result)
    {
        if (result != FMOD.RESULT.OK)
            throw new Exception($"FMOD : {result}");
    }
}