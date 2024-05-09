using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{

    public class TheRender : MonoBehaviour
    {
        private Light dir_light;
        private Quaternion start_rot;
        private float update_timer = 0f;

        void Start()
        {
            //Light
            GameData gdata = GameData.Get();
            bool is_night = TheGame.Get().IsNight();
            dir_light = GetDirectionalLight();

            float target = is_night ? gdata.night_light_ambient_intensity : gdata.day_light_ambient_intensity;
            float light_angle = PlayerData.Get().day_time * 360f / 24f;
            RenderSettings.ambientIntensity = target;
            if (dir_light != null && dir_light.type == LightType.Directional)
            {
                start_rot = dir_light.transform.rotation;
                dir_light.intensity = is_night ? gdata.night_light_dir_intensity : gdata.day_light_dir_intensity;
                dir_light.shadowStrength = is_night ? 0f : 1f;
                if (gdata.rotate_shadows)
                    dir_light.transform.rotation = Quaternion.Euler(0f, light_angle + 180f, 0f) * start_rot;
            }
        }

        void Update()
        {
            GameData gdata = GameData.Get();
            bool is_night = TheGame.Get().IsNight();
            float light_mult = GetLightMult();
            float target = is_night ? gdata.night_light_ambient_intensity : gdata.day_light_ambient_intensity;
            float light_angle = PlayerData.Get().day_time * 360f / 24f;
            RenderSettings.ambientIntensity = Mathf.MoveTowards(RenderSettings.ambientIntensity, target * light_mult, 0.2f * Time.deltaTime);

            // Рассчитываем цвет в зависимости от времени суток
            Color dayColor = new Color(1f, 0.96f, 0.84f); // Цвет FFF4D6
            Color nightColor = new Color(0.2f, 0.1f, 0.3f);
            Color targetColor = is_night ? nightColor : dayColor;
            Color currentColor = RenderSettings.ambientLight;
            RenderSettings.ambientLight = Color.Lerp(currentColor, targetColor, Time.deltaTime * 0.5f); // Плавный переход цвета с более мягким параметром скорости

            if (dir_light != null && dir_light.type == LightType.Directional)
            {
                float dtarget = is_night ? gdata.night_light_dir_intensity : gdata.day_light_dir_intensity;
                dir_light.intensity = Mathf.MoveTowards(dir_light.intensity, dtarget * light_mult, 0.2f * Time.deltaTime);
                dir_light.shadowStrength = Mathf.MoveTowards(dir_light.shadowStrength, is_night ? 0f : 1f, 0.2f * Time.deltaTime);
                if (gdata.rotate_shadows)
                    dir_light.transform.rotation = Quaternion.Euler(0f, light_angle + 180f, 0f) * start_rot;
            }

            // Добавляем эффект ночной дымки у земли
            if (is_night)
            {
                // Настройки эффекта дымки
                float fogDensity = 0.05f; // Плотность дымки
                float fogHeight = 0.2f; // Высота начала дымки над землей
                float fogHeightScale = 0.5f; // Масштаб высоты дымки

                // Расчет эффекта дымки
                float playerHeight = 1.8f; // Высота игрока над землей
                float fogFactor = Mathf.Clamp01((playerHeight - fogHeight) * fogHeightScale); // Рассчитываем фактор дымки
                RenderSettings.fogDensity = fogDensity * fogFactor; // Устанавливаем плотность дымки
            }
            else
            {
                // Если не ночь, отключаем эффект дымки
                RenderSettings.fogDensity = 0f;
            }

            update_timer += Time.deltaTime;
            if (update_timer > GameData.Get().optim_refresh_rate)
            {
                update_timer = 0f;
                SlowUpdate();
            }
        }


        void SlowUpdate()
        {
            //Optimization Loop
            Vector3 center_pos = CameraController.Get().GetTargetPosOffsetFace(GameData.Get().optim_facing_offset);
            float dist_mult = GameData.Get().optim_distance_multiplier;
            bool turn_off_obj = GameData.Get().optim_turn_off_gameobjects;
            List<Selectable> selectables = Selectable.GetAll();

            foreach(Selectable select in selectables)
            {
                float dist = (select.GetPosition() - center_pos).magnitude;
                select.SetActive(dist < select.active_range * dist_mult, turn_off_obj);
            }
        }

        public Light GetDirectionalLight()
        {
            foreach (Light light in FindObjectsOfType<Light>())
            {
                if (light.type == LightType.Directional && light.gameObject.tag == "Sun")
                    return light;
            }
            return null;
        }

        public float GetLightMult()
        {
            if (WeatherSystem.Get())
                return WeatherSystem.Get().GetLightMult();
            return 1f;
        }
    }

}