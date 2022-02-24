using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class camionComponent : MonoBehaviour
{
    public GameObject cocheGO;
    public float turnSpeed = 50f;
    private int etapa = 1;
    public float _Velocity = 0.0f;      // Current Travelling Velocity
    public float _MaxVelocity = 1.0f;   // Maxima Velocity
    public float _Acc = 0.0f;           // Current Acceleration
    public float _AccSpeed = 0.0001f;      // Amount to increase Acceleration with.
    public float _DesaccSpeed = 2e-07f;// Amount to decrease Acceleration with.
    public float _MaxAcc = 1.0f;        // Max Acceleration
    public float _MinAcc = -1.0f;       // Min Acceleration
    public float segundosMovimientos;

    public Text txtVelocidad;
    public Text txtAceleracion;
    public Text txtTiempo;
    public Text txtDistancia;
    public Text txtMovimiento;

    public bool detenido = false;
    private float factorVelocidadReferencia = 20.0f;
    private float velocidadEnReferencia = 0.0f;
    private float aceleracionEnReferencia = 0.0f;
    private float distanciaEnReferencia = 0.0f;
    private float distanciaRecPrimerTramo = 0.0f;
    private float distanciaRecSegundoTramo = 0.0f;
    private float distanciaRecTercerTramo = 0.0f;
    private float distanciaTotal = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        cocheGO = GameObject.FindObjectOfType<Camion>().gameObject;
        _Velocity = 0.01f;
        txtDistancia.text = "";
        Update();
    }


    //al ser la velocidad maxima 1.0f, entonces la velocidad en referencia a km/h va a ser 1.0/20;

    // Update is called once per frame
    void Update()
    {
        if (!detenido)
        {

            segundosMovimientos += Time.deltaTime;
            velocidadEnReferencia = _Velocity * factorVelocidadReferencia;
            //si el tiempo es menor a 5 segunddos,  me muevo en movimiento rectilineo uniforme acelerado
            if (segundosMovimientos < 5)
            {
                _Acc += _AccSpeed;
                //velocidad inicial 0, velocidad final la actual, tiempo inicial 0, tiempo final actual
                //pasamos la velocidad a metros sobre segundos y dividimos por la cantidad de segundos
                aceleracionEnReferencia = (velocidadEnReferencia / 3.6f) / (segundosMovimientos);
                //X= a.t /2 esta ecuación nos permite calcular la distancia recorrida en el mrua sin rapidez inicial.
                distanciaRecPrimerTramo = (aceleracionEnReferencia * (segundosMovimientos * segundosMovimientos) / 2);
                //txtDistancia.text = "Distancia Primer Tramo: " + " " + distanciaRecPrimerTramo.ToString();
                txtMovimiento.text = "MOVIMIENTO RECTILINEO UNIFORME ACELERADO";
            }


            //si el tiempo esta entre 5 y 11 segundos me muevo en movimiento rectilineo uniforme
            if (segundosMovimientos >= 5 && segundosMovimientos < 11)
            {
                _Acc = 0.0f;
                //la aceleracion es 0 cuando la velocidad es constante
                aceleracionEnReferencia = 0.0f;
                //X= Vo.t (t en funcion desde que la velocidad es constante
                distanciaRecSegundoTramo = (5.56f * (segundosMovimientos - 5));
                //txtDistancia.text = "Distancia Segundo Tramo: " + " " + distanciaRecSegundoTramo.ToString();
                txtMovimiento.text = "MOVIMIENTO RECTILINEO UNIFORME";
            }


            //si el tiempo es mayor a 11 segundos y menor a 20s me muevo desacelerando
            if (segundosMovimientos >= 11 && segundosMovimientos < 20)
            {
                if (_Velocity > 0.0f)
                {
                    _Acc -= _DesaccSpeed;
                }
                else
                {
                    _Acc = 0.0f;
                }
                
                //usamos la misma formula
                //velocidad inicial 0, velocidad final la actual, tiempo inicial 0, tiempo final actual
                //pasamos la velocidad a metros sobre segundos y dividimos por la cantidad de segundos
                //multiplicamos la maxima velocidad por la de referencia que es la velocidad inicial cuando empieza a frenar
                //dividimos por 3.6 para pasar a metros/s
                //dividimos por los segudos actuales menos los 11 segundos de tiempo inicial de frenado
                aceleracionEnReferencia = ((velocidadEnReferencia - (_MaxVelocity * factorVelocidadReferencia)) / 3.6f) / (segundosMovimientos - 11);

                //X= Vo.t-a.t /2.
                distanciaRecTercerTramo = ((velocidadEnReferencia * (segundosMovimientos - 11)) + ((aceleracionEnReferencia * ((segundosMovimientos - 11) * (segundosMovimientos - 11))) / 2));
                txtMovimiento.text = "MOVIMIENTO RECTILINEO DESACELERADO";
            }

                //verificamos a la vez que no sobrepase la velocidad máxima ni la minima
                if (_Velocity <= _MaxVelocity && _Velocity >= 0.01f)
            {
                _Velocity += _Acc;
            }
            else if (_Velocity > _MaxVelocity)
            {
                _Velocity = _MaxVelocity;
            }
            else if (_Velocity < 0.01f)
            {
                _Velocity = 0.0f;
            }


            //si pasaron los 20 segundos detengo el camion
            if (segundosMovimientos >= 20)
            {
                _Acc = 0;
                _Velocity = 0.0f;
                aceleracionEnReferencia = 0.0f;
                detenido = true;
                txtDistancia.text = "Distancia Total Recorrida: 50 m";
                txtMovimiento.text = "VEHÍCULO ESTÁTICO";
            }

            //movimmiento en eje x del camion
            transform.Translate(Vector2.right * _Velocity * Time.deltaTime);
            txtTiempo.text = "Tiempo:" + " " + segundosMovimientos.ToString("f0");
            txtVelocidad.text = "Velocidad: " + " " + velocidadEnReferencia.ToString();
            txtAceleracion.text = "Aceleración: " + " " + aceleracionEnReferencia.ToString();

        }

    }
}
