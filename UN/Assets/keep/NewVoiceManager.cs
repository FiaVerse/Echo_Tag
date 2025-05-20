using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Meta.WitAi;
using Meta.WitAi.CallbackHandlers;
using Meta.WitAi.Json;
using Meta.WitAi.TTS.Integrations;   // ← for TTSSpeaker
using Oculus.Voice;

public class NewVoiceManager : MonoBehaviour
{
    [Header("Wit Configuration")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;
    [SerializeField] private WitResponseMatcher responseMatcher;

    [Header("TTS (Text-to-Speech)")]
    [SerializeField] private Meta.WitAi.TTS.Utilities.TTSSpeaker ttsSpeaker;      // ← assign your TTSSpeaker here

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI transcriptionText;
    [SerializeField] private TextMeshProUGUI composerResponseText;

    [Header("Voice Events")]
    [SerializeField] private UnityEvent wakeWordDetected;
    [SerializeField] private UnityEvent<string> completeTranscription;

    private bool _voiceCommandReady;

    private void Awake()
    {
        // Register voice events
        appVoiceExperience.VoiceEvents.OnRequestCompleted     .AddListener(ReactivateVoice);
        appVoiceExperience.VoiceEvents.OnPartialTranscription .AddListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription    .AddListener(OnFullTranscription);
       // appVoiceExperience.VoiceEvents.OnResponse             .AddListener(HandleComposerResponse);

        // Wake-word matcher hookup
        var eventField = typeof(WitResponseMatcher)
            .GetField("onMultiValueEvent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (eventField != null && eventField.GetValue(responseMatcher) is MultiValueEvent onMulti)
            onMulti.AddListener(WakeWordDetected);

        // Start listening immediately
        appVoiceExperience.Activate();
    }

    private void OnDestroy()
    {
        appVoiceExperience.VoiceEvents.OnRequestCompleted     .RemoveListener(ReactivateVoice);
        appVoiceExperience.VoiceEvents.OnPartialTranscription .RemoveListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription    .RemoveListener(OnFullTranscription);
        //appVoiceExperience.VoiceEvents.OnResponse             .RemoveListener(HandleComposerResponse);

        var eventField = typeof(WitResponseMatcher)
            .GetField("onMultiValueEvent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (eventField != null && eventField.GetValue(responseMatcher) is MultiValueEvent onMulti)
            onMulti.RemoveListener(WakeWordDetected);
    }

    private void ReactivateVoice() => appVoiceExperience.Activate();

    private void WakeWordDetected(string[] _) 
    {
        _voiceCommandReady = true;
        wakeWordDetected?.Invoke();
        Debug.Log("[VoiceManager] Wake Word Detected");
    }

    private void OnPartialTranscription(string transcription)
    {
        if (!_voiceCommandReady) return;
        if (transcriptionText != null)
            transcriptionText.text = transcription;
    }

    private void OnFullTranscription(string transcription)
    {
        if (!_voiceCommandReady) return;
        _voiceCommandReady = false;
        completeTranscription?.Invoke(transcription);
        Debug.Log("[VoiceManager] Full Transcription: " + transcription);
    }

   /* private void HandleComposerResponse(WitResponseNode response)
    {
        // 1) Log full raw JSON
        Debug.Log("[WIT RAW RESPONSE] " + response.ToString());

        // 2) Extract & log the 'text' reply
        string composerReply = response["text"];
        if (!string.IsNullOrEmpty(composerReply))
        {
            Debug.Log("[Composer Response] " + composerReply);

            // 3) Update UI
            if (composerResponseText != null)
                composerResponseText.text = composerReply;

            // 4) Speak it via TTS
            if (ttsSpeaker != null)
            {
                Debug.Log("[TTS] Speaking: " + composerReply);
                ttsSpeaker.Speak(composerReply);
            }
            else
            {
                Debug.LogWarning("[TTS] No TTSSpeaker component assigned!");
            }
        }
        else
        {
            Debug.LogWarning("[Composer Response] No 'text' field found in response.");
        }
    }*/
}
