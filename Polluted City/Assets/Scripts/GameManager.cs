using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //pra poder fazer o carregamento das cenas
using System; //importada para salvar dados
using System.Runtime.Serialization.Formatters.Binary; //formato para salvar em binario
using System.IO;
using Random = UnityEngine.Random; // declarando que o random que usamos é o da unity e nao da system

[Serializable]
public class PlayerData //classe que vou usar para salvar os dados
{ // aqui eu tenho que declarar tudo que vou querer salvar
    public int coins;
    //vou salvar todos os dados da missao separadamente
    public int[] max; //coloquei em vetor por que são 2 missoes para salvar
    public int[] progress;
    public int[] currentProgress;
    public int[] reward;
    public string[] missionType;
	public int[] characterCost;
}

public class GameManager : MonoBehaviour
{
    public static GameManager gm; // variavel para acessar tudo oq tem dentro do gamemanager

    public int coins;

	public int[] characterCost;

	public int characterIndex;

    public MissionBase[] missions; //variavel para pegar referencia as missoes que vou criar

    private string filePath; // caminho do arquivo
    // Start is called before the first frame update
    private void Awake() // awake sera chamado antes do start
    {
        if(gm == null) // se gamemanager for nulo
        {
            gm = this; // transforma esse objeto como gm
        }
        else if(gm != this) // se gm nao for esse objeto
        {
            Destroy(gameObject); // destroi ele entao, dessa forma nos garante que tenha 1 game manager só
        }
        DontDestroyOnLoad(gameObject);

        filePath = Application.persistentDataPath + "/playerInfo.dat"; // definindo o caminho do arquivo e passando pra variavel ''persistentDataPatch'' é um caminho padrao da unity, a string a seguir é onome do arquivo

        missions = new MissionBase[2]; //instanciando as missoes

        if (File.Exists(filePath)) // verificando se um arquivo save exist
		{
			Load(); // se existir ele carrega
		}
        else // se nao ele cria as missao do 0
		{
			for (int i = 0; i < missions.Length; i++)
			{
				GameObject newMission = new GameObject("Mission" + i);
				newMission.transform.SetParent(transform);
				MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeter, MissionType.FishesSingleRun };
				int randomType = Random.Range(0, missionType.Length);
				if (randomType == (int)MissionType.SingleRun)
				{
					missions[i] = newMission.AddComponent<SingleRun>();

				}
				else if (randomType == (int)MissionType.TotalMeter)
				{
					missions[i] = newMission.AddComponent<TotalMeters>();

				}
				else if (randomType == (int)MissionType.FishesSingleRun)
				{
					missions[i] = newMission.AddComponent<FishesSingleRun>();

				}

				missions[i].Created();
			}
		}
    }   

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter(); // formato binario para salvar os dados
        FileStream file = File.Create(filePath); // criando o arquivo no local escolhido

        PlayerData data = new PlayerData(); // a partir de agora começamos a salvar os dados
        
        data.coins = coins;
        data.max = new int[2];
        data.progress = new int[2];
        data.reward = new int[2];
        data.currentProgress = new int[2];
        data.missionType = new string[2];
		data.characterCost = new int[characterCost.Length];

        for (int i = 0; i < 2; i++) // aqui estamos dando save nos parametros, no caso o 2 do for, é igual a 3, ja que começamos do 0 1 2, e temos 3 missoes
        {
            data.max[i] = missions[i].max;
		    data.progress[i] = missions[i].progress;
		    data.currentProgress[i] = missions[i].currentProgress;
		    data.reward[i] = missions[i].reward;
		    data.missionType[i] = missions[i].missionType.ToString();
        }

		for (int i = 0; i < characterCost.Length; i++)
		{
			data.characterCost[i] = characterCost[i]; // salvando o custo do personagem
		}

        bf.Serialize(file, data); // ele passa tudo que em data, para o arquivo que a gente criou, meio que criptografa
		file.Close(); // fechamos o save
    }

    void Load() // sem necessidade de está publica 
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(filePath, FileMode.Open); // abrindo o arquivo

		PlayerData data = (PlayerData)bf.Deserialize(file); // descriptografando os dados, trazendo pro jogo
		file.Close(); // fecha o arquivo
        //distribuindo os dados
		coins = data.coins;

		for (int i = 0; i < 2; i++)
		{
			GameObject newMission = new GameObject("Mission" + i);
			newMission.transform.SetParent(transform);
			if(data.missionType[i] == MissionType.SingleRun.ToString())
			{
				missions[i] = newMission.AddComponent<SingleRun>();
				missions[i].missionType = MissionType.SingleRun;
			}
			else if (data.missionType[i] == MissionType.TotalMeter.ToString())
			{
				missions[i] = newMission.AddComponent<TotalMeters>();
				missions[i].missionType = MissionType.TotalMeter;
			}
			else if (data.missionType[i] == MissionType.FishesSingleRun.ToString())
			{
				missions[i] = newMission.AddComponent<FishesSingleRun>();
				missions[i].missionType = MissionType.FishesSingleRun;
			}

			missions[i].max = data.max[i];
			missions[i].progress = data.progress[i];
			missions[i].currentProgress = data.currentProgress[i];
			missions[i].reward = data.reward[i];
		}

		for (int i = 0; i < data.characterCost.Length; i++)
		{
			characterCost[i] = data.characterCost[i];
		}
	}

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRun(int charIndex) // função para chamar a cena game
    {
		characterIndex = charIndex;
        SceneManager.LoadScene("Game");
		Cursor.visible = false;
    }

    public void EndRun() // função para chamar a cena menu
    {
        SceneManager.LoadScene("Menu");
    }

	public void QuitGame()
	{
		Application.Quit();
	}

    public MissionBase GetMission(int index) // função para o menu poder pegar a missao
	{
		return missions[index];
	}

    public void StartMissions() // função para contabilizar o progresso da missao
    {
        for (int i = 0; i < 2; i++)
        {
            missions[i].RunStart();
        }
    }

    public void GenerateMission(int i)
    {
        Destroy(missions[i].gameObject);

		GameObject newMission = new GameObject("Mission" + i);
		newMission.transform.SetParent(transform);
		MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeter, MissionType.FishesSingleRun };
		int randomType = Random.Range(0, missionType.Length);
		if (randomType == (int)MissionType.SingleRun)
		{
			missions[i] = newMission.AddComponent<SingleRun>();

		}
		else if (randomType == (int)MissionType.TotalMeter)
		{
			missions[i] = newMission.AddComponent<TotalMeters>();

		}
		else if (randomType == (int)MissionType.FishesSingleRun)
		{
			missions[i] = newMission.AddComponent<FishesSingleRun>();

		}

		missions[i].Created();

        FindObjectOfType<Menu>().SetMission(); // chamando a função para o menu atualizar os valores da missao na tela
			
    }
}
