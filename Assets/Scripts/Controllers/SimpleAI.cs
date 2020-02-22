using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SimpleAI : MonoBehaviour
{

    public EnemyAI _cabeca;
    NavMeshAgent _navMesh;
    Transform alvo;
    Vector3 posicInicialDaAI;
    Vector3 ultimaPosicConhecida;
    float timerProcura;

    enum estadoDaAI
    {
        parado, seguindo, procurandoAlvoPerdido
    };
    estadoDaAI _estadoAI = estadoDaAI.parado;

    void Start()
    {

        _navMesh = GetComponent<NavMeshAgent>();
        alvo = null;
        ultimaPosicConhecida = Vector3.zero;
        _estadoAI = estadoDaAI.parado;
        posicInicialDaAI = transform.position;
        timerProcura = 0;
    }

    void Update()
    {
        if (_cabeca)
        {
            switch (_estadoAI)
            {
                case estadoDaAI.parado:
                    _navMesh.SetDestination(posicInicialDaAI);
                    if (_cabeca.inimigosVisiveis.Count > 0)
                    {
                        alvo = _cabeca.inimigosVisiveis[0];
                        ultimaPosicConhecida = alvo.position;
                        _estadoAI = estadoDaAI.seguindo;
                    }
                    break;
                case estadoDaAI.seguindo:
                    _navMesh.SetDestination(alvo.position);
                    if (!_cabeca.inimigosVisiveis.Contains(alvo))
                    {
                        ultimaPosicConhecida = alvo.position;
                        _estadoAI = estadoDaAI.procurandoAlvoPerdido;
                    }
                    break;
                case estadoDaAI.procurandoAlvoPerdido:
                    _navMesh.SetDestination(ultimaPosicConhecida);
                    timerProcura += Time.deltaTime;
                    if (timerProcura > 5)
                    {
                        timerProcura = 0;
                        _estadoAI = estadoDaAI.parado;
                        break;
                    }
                    if (_cabeca.inimigosVisiveis.Count > 0)
                    {
                        alvo = _cabeca.inimigosVisiveis[0];
                        ultimaPosicConhecida = alvo.position;
                        _estadoAI = estadoDaAI.seguindo;
                    }
                    break;
            }
        }
    }
}