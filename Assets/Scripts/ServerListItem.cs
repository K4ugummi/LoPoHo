using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class ServerListItem : MonoBehaviour {

    public delegate void JoinServerDelegate(MatchInfoSnapshot _matchInfo);
    private JoinServerDelegate joinServerCallback;

    [SerializeField]
    private Text serverNameText;

    private MatchInfoSnapshot matchInfo;

    public void Setup(MatchInfoSnapshot _matchInfo, JoinServerDelegate _joinServerCallback) {
        matchInfo = _matchInfo;
        joinServerCallback = _joinServerCallback;

        serverNameText.text = matchInfo.name + " (" + matchInfo.currentSize + "/" + matchInfo.maxSize + ")";
    }

    public void JoinServer() {
        joinServerCallback.Invoke(matchInfo);
    }

}
