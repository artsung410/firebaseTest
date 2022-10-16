using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    private static GameManager instance;

    public TextMeshProUGUI scoreText;
    public Transform[] spawnPositions; // 플레이어가 생성할 위치
    public GameObject playerPrefab; // 생성할 플레이어의 원형 프리팹
    public GameObject ballPrefab; // 생성할 오브젝트들의 원형 프리팹

    private int[] playerScores;

    private void Start()
    {
        playerScores = new[] { 0, 0 };

        // 각각의 플레이어가 한번 실행됨.
        SpawnPlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnBall(); // 방장만 볼을 생성하게 됨.
        }
    }

    private void SpawnPlayer()
    {
        // 현재 방에 들어온 로컬 플레이어의 나 자신의 번호를 가져온다.
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        // a플레이어 세상에서 a플레이어를 생성함, 그다음에 b c d 세상에 a의 복제본이 생성됨.
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.identity);
    }

    private void SpawnBall()
    {
        //PhotonNetwork.Instantiate(ballPrefab.name, Vector2.zero, Quaternion.identity).GetComponent<Ball>();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void AddScore(int playerNumber, int score)
    {
        playerScores[playerNumber - 1] += score;

        // RpcTarget : 어떤 클라이언트에게 동기화를 징행할 것인지, All이면 모든 클라이언트들에게 동기화 진행.
        photonView.RPC("RPCUpdateScoreText", RpcTarget.All, playerScores[0].ToString(), playerScores[1].ToString());
    }


    [PunRPC]
    private void RPCUpdateScoreText(string player1ScoreText, string player2ScoreText)
    {
        scoreText.text = $"{player1ScoreText} : {player2ScoreText}";
    }
}