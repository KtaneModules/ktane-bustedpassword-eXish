using System.Collections;
using UnityEngine;
using System;

public class BustedPasswordScript : MonoBehaviour {

    public KMAudio audio;
    public KMBombInfo bomb;
    public KMSelectable[] buttons;

    public AudioSource staticAudio;
    public Transform needleRotator;
    public MeshRenderer ledMesh;
    public Light ledLight;
    public Material[] ledMats;

    private string[] table = { "abyss", "azure", "bench", "block", "brick", "clump", "dummy", "eager", "elbow", "forge", "gecko", "hotel", "igloo", "index", "labor", "leech", "logic", "major", "movie", "ninja", "occur", "ocean", "pixel", "plank", "psalm", "pylon", "quote", "squid", "swarm", "tango", "upset", "valve", "verge", "waist", "yacht", "zebra" };
    private int[] sectionOrder = { 0, 1, 2, 3, 4 };
    private int[] submission = new int[2];
    private bool moveNeedle = true;
    private int selectedWord = -1;
    private float needleSpeed = .25f;
    private Coroutine staticCo;
    private Coroutine timerCo;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable obj in buttons)
        {
            KMSelectable pressed = obj;
            pressed.OnInteract += delegate () { PressButton(pressed); return false; };
        }
    }

    void Start()
    {
        ledLight.range *= transform.lossyScale.x;
        selectedWord = UnityEngine.Random.Range(0, table.Length);
        sectionOrder = sectionOrder.Shuffle();
        Debug.LogFormat("[Busted Password #{0}] The sections from top to bottom output the letters {1}, {2}, {3}, {4} and {5}.", moduleId, table[selectedWord][sectionOrder[0]], table[selectedWord][sectionOrder[1]], table[selectedWord][sectionOrder[2]], table[selectedWord][sectionOrder[3]], table[selectedWord][sectionOrder[4]]);
        Debug.LogFormat("[Busted Password #{0}] These letters form the word {1}, which in xy format is {2}{3}.", moduleId, table[selectedWord], selectedWord % 6 + 1, selectedWord / 6 + 1);
        StartCoroutine(RotateNeedle());
    }

    void PressButton(KMSelectable pressed)
    {
        if (moduleSolved != true)
        {
            int index = Array.IndexOf(buttons, pressed);
            if (index == 0)
            {
                pressed.AddInteractionPunch();
                audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, pressed.transform);
                moveNeedle = !moveNeedle;
                if (!moveNeedle)
                {
                    int section = -1;
                    if (needleRotator.localEulerAngles.y <= 15.5f)
                        section = 4;
                    else if (needleRotator.localEulerAngles.y >= 30.5f && needleRotator.localEulerAngles.y <= 60.5f)
                        section = 3;
                    else if (needleRotator.localEulerAngles.y >= 75.5f && needleRotator.localEulerAngles.y <= 105.5f)
                        section = 2;
                    else if (needleRotator.localEulerAngles.y >= 120.5f && needleRotator.localEulerAngles.y <= 150.5f)
                        section = 1;
                    else if (needleRotator.localEulerAngles.y >= 165.5f)
                        section = 0;
                    if (section != -1)
                    {
                        staticAudio.Play();
                        staticCo = StartCoroutine(PlayStaticCode(table[selectedWord][sectionOrder[section]]));
                    }
                }
                else if (staticAudio.isPlaying)
                {
                    staticAudio.Stop();
                    staticAudio.volume = .1f;
                    StopCoroutine(staticCo);
                }
            }
            else
            {
                pressed.AddInteractionPunch(0.5f);
                audio.PlaySoundAtTransform("smallclick", pressed.transform);
                if (submission[0] == 0)
                {
                    submission[0] = 1;
                    submission[1] = 1;
                    ledMesh.material = ledMats[2];
                    ledLight.color = Color.yellow;
                    ledLight.enabled = true;
                }
                else
                {
                    submission[index - 1]++;
                    if (submission[index - 1] == 7)
                        submission[index - 1] = 1;
                }
                if (timerCo != null)
                    StopCoroutine(timerCo);
                timerCo = StartCoroutine(TimerRoutine());
            }
        }
    }

    IEnumerator RotateNeedle()
    {
        Vector3 needleDown = new Vector3(0, 0, 0);
        Vector3 needleUp = new Vector3(0, 180, 0);
        while (true)
        {
            float t = 0;
            while (t < 1f)
            {
                yield return null;
                t += Time.deltaTime * needleSpeed;
                needleRotator.localEulerAngles = Vector3.Lerp(needleDown, needleUp, t);
                while (!moveNeedle) yield return null;
            }
            t = 0;
            while (t < 1f)
            {
                yield return null;
                t += Time.deltaTime * needleSpeed;
                needleRotator.localEulerAngles = Vector3.Lerp(needleUp, needleDown, t);
                while (!moveNeedle) yield return null;
            }
        }
    }

    IEnumerator PlayStaticCode(char letter)
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            switch (letter)
            {
                case 'a':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'b':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'c':
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;
                    break;
                case 'd':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;
                    break;
                case 'e':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'f':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'g':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;
                    break;
                case 'h':
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'i':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'j':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'k':
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;
                    break;
                case 'l':
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'm':
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'n':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'o':
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;
                    break;
                case 'p':
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'q':
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'r':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;
                    break;
                case 's':
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;
                    break;
                case 't':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'u':
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;
                    break;
                case 'v':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'w':
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
                case 'x':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;
                    break;
                case 'y':
                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .15f;
                    yield return new WaitForSeconds(.5f);
                    staticAudio.volume = .1f;
                    break;
                default:
                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;

                    yield return new WaitForSeconds(.5f);

                    staticAudio.volume = .2f;
                    yield return new WaitForSeconds(.25f);
                    staticAudio.volume = .1f;
                    break;
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(3);
        if ((submission[0] == selectedWord % 6 + 1) && (submission[1] == selectedWord / 6 + 1))
        {
            moduleSolved = true;
            Debug.LogFormat("[Busted Password #{0}] Submitted {1}{2} which is correct. Module solved!", moduleId, submission[0], submission[1]);
            GetComponent<KMBombModule>().HandlePass();
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CapacitorPop, transform);
            ledMesh.material = ledMats[0];
            ledLight.color = Color.green;
            if (staticAudio.isPlaying)
            {
                staticAudio.Stop();
                StopCoroutine(staticCo);
            }
            for (int i = 0; i < 25; i++)
            {
                yield return new WaitForSeconds(.05f);
                needleSpeed -= .01f;
            }
        }
        else
        {
            Debug.LogFormat("[Busted Password #{0}] Submitted {1}{2} which is incorrect. Strike!", moduleId, submission[0], submission[1]);
            GetComponent<KMBombModule>().HandleStrike();
            submission = new int[2];
            timerCo = null;
            ledMesh.material = ledMats[1];
            ledLight.color = Color.red;
        }
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} stop <#> [Stops the needle on section '#'] | !{0} start [Starts moving the needle] | !{0} submit <x><y> [Submits the specified password in xy format] | Valid sections are 1-5 from top to bottom";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        if (command.EqualsIgnoreCase("start"))
        {
            if (moveNeedle)
            {
                yield return "sendtochaterror The needle is already moving!";
                yield break;
            }
            yield return null;
            buttons[0].OnInteract();
            yield break;
        }
        string[] parameters = command.Split(' ');
        if (parameters[0].EqualsIgnoreCase("stop"))
        {
            if (parameters.Length > 2)
                yield return "sendtochaterror Too many parameters!";
            else if (parameters.Length == 2)
            {
                int temp;
                if (!int.TryParse(parameters[1], out temp))
                {
                    yield return "sendtochaterror!f The specified section '" + parameters[1] + "' is invalid!";
                    yield break;
                }
                if (temp < 1 || temp > 5)
                {
                    yield return "sendtochaterror The specified section '" + parameters[1] + "' is invalid!";
                    yield break;
                }
                if (!moveNeedle)
                {
                    yield return "sendtochaterror The needle is already stopped!";
                    yield break;
                }
                yield return null;
                while (true)
                {
                    int section;
                    if (needleRotator.localEulerAngles.y <= 15.5f)
                        section = 4;
                    else if (needleRotator.localEulerAngles.y >= 30.5f && needleRotator.localEulerAngles.y <= 60.5f)
                        section = 3;
                    else if (needleRotator.localEulerAngles.y >= 75.5f && needleRotator.localEulerAngles.y <= 105.5f)
                        section = 2;
                    else if (needleRotator.localEulerAngles.y >= 120.5f && needleRotator.localEulerAngles.y <= 150.5f)
                        section = 1;
                    else if (needleRotator.localEulerAngles.y >= 165.5f)
                        section = 0;
                    else
                        section = -1;
                    if (section != temp - 1)
                        yield return "trycancel";
                    else
                        break;
                }
                buttons[0].OnInteract();
            }
            else
                yield return "sendtochaterror Please specify a section to stop the needle on!";
            yield break;
        }
        if (parameters[0].EqualsIgnoreCase("submit"))
        {
            if (parameters.Length > 2)
                yield return "sendtochaterror Too many parameters!";
            else if (parameters.Length == 2)
            {
                if (parameters[1].Length != 2 || !parameters[1][0].EqualsAny('1', '2', '3', '4', '5', '6') || !parameters[1][1].EqualsAny('1', '2', '3', '4', '5', '6'))
                {
                    yield return "sendtochaterror!f The specified password '" + parameters[1] + "' is invalid!";
                    yield break;
                }
                yield return null;
                while (submission[0] != int.Parse(parameters[1][0].ToString()))
                {
                    buttons[1].OnInteract();
                    yield return new WaitForSeconds(.1f);
                }
                while (submission[1] != int.Parse(parameters[1][1].ToString()))
                {
                    buttons[2].OnInteract();
                    yield return new WaitForSeconds(.1f);
                }
                if ((submission[0] != selectedWord % 6 + 1) || (submission[1] != selectedWord / 6 + 1))
                    yield return "strike";
                else
                    yield return "solve";
            }
            else
                yield return "sendtochaterror Please specify a password to submit!";
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        while (submission[0] != selectedWord % 6 + 1)
        {
            buttons[1].OnInteract();
            yield return new WaitForSeconds(.1f);
        }
        while (submission[1] != selectedWord / 6 + 1)
        {
            buttons[2].OnInteract();
            yield return new WaitForSeconds(.1f);
        }
        while (!moduleSolved) yield return true;
    }
}