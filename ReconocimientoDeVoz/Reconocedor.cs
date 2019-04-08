using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;
using UnityEngine;

namespace ReconocimientoDeVoz
{
    public class Reconocedor
    {

        public static bool grabando = false;

        public static async Task<List<string>> reconocerVoz(int tiempo)
        {
            List<string> listaSoluciones = new List<string>();
            if (NAudio.Wave.WaveIn.DeviceCount < 1)
            {
                Debug.Log("Sin microfono");
                return listaSoluciones;
            }
            var speech = SpeechClient.Create();
            var streamingCall = speech.StreamingRecognize();


            //Configuración de petición inicial
            await streamingCall.WriteAsync(
                new StreamingRecognizeRequest()
                {
                    StreamingConfig = new StreamingRecognitionConfig()
                    {
                        Config = new RecognitionConfig()
                        {
                            Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                            SampleRateHertz = 16000,
                            LanguageCode = "es-ES",
                        },
                        InterimResults = true,
                        SingleUtterance = true //dejará de reconocer cuando se detecte que se ha dejado de hablar
                    }
                }
                );

            //Muestra las respuestas cuando llegan
            Task pintaRespuestas = Task.Run(async () =>
            {
                while (await streamingCall.ResponseStream.MoveNext(default(CancellationToken)))
                {
                    foreach (var result in streamingCall.ResponseStream.Current.Results)
                    {
                        foreach (var alternative in result.Alternatives)
                        {
                            Debug.Log(alternative.Transcript);
                            listaSoluciones.Add(alternative.Transcript);
                        }
                    }
                }
            });


            //leer de microfono y enviar a la API
            object writeLock = new object();
            bool writeMore = true;
            var waveIn = new NAudio.Wave.WaveInEvent();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new NAudio.Wave.WaveFormat(16000, 1);
            waveIn.DataAvailable += (object sender, NAudio.Wave.WaveInEventArgs args) =>
            {
                lock (writeLock)
                {
                    if (!writeMore)
                    {
                        return;
                    }
                    streamingCall.WriteAsync(
                        new StreamingRecognizeRequest()
                        {
                            AudioContent = Google.Protobuf.ByteString.CopyFrom(args.Buffer, 0, args.BytesRecorded)
                        }
                        ).Wait();
                }
            };
            waveIn.StartRecording();
            Debug.Log("Habla");
            grabando = true;
            await Task.Delay(TimeSpan.FromSeconds(tiempo));
            //deja de grabar y termina
            waveIn.StopRecording();
            grabando = false;
            lock (writeLock) writeMore = false;
            await streamingCall.WriteCompleteAsync();
            await pintaRespuestas;
            await SpeechClient.ShutdownDefaultChannelsAsync();

            return listaSoluciones;
        }



    }
}
