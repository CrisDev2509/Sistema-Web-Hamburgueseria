import http from 'k6/http';
import { check } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

export let options = {
  vus: 10,
    duration: '10s',
}

export default function () {
  const url = 'http://localhost:5139/Sale/Index';
    const params = {
        headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
        'cookie': '.AspNetCore.Antiforgery.RoiQpDN09kQ=CfDJ8I96uG1f--1BoPXXfUOVC1uso03HyKMhoU3bWT4uIfWtskzVJZvS1aTJOfEXOopZAhVQGDb58MGze_pn4Bg1pIc7T1xI7f2qJsF6kOGyXwGsoC938fvx2NONLYFXjBPZodD65uft1AK4IrDECGJN55U; .AspNetCore.Cookies=CfDJ8I96uG1f--1BoPXXfUOVC1v9bIFypve6tyojFNscu3EoutQXRJKE-7me2fhqj39_MVyaqTK8aIVVWIAR6SK7EJtolXvM0xScDikM2aDROowAzX0iwwrUtGLqJQCIO7E1pQN9G8CpSh3c3zo3fo7OHu1VMFKlfRm2ehSNAUjeZRZ0QMiCQ_8YkoE6Saee9LdrYhcM9DI00Z7a3KNiHudFSfv2nz8kLVy3if3JI_D_otWEwqzJx3DUzhmHuYnshC1bhUFwqob-xnbbldOb4stJbqCCMQzH1WGXtP04rfzF6sOKwR85GTf2m1toJXNjAVS64-x_th-61wXhJM3Ddo_PQBHxzH_FHJRBFwbbf-dmC1ZzXWzyRZsSnNKVhXdfK0RPYIWqpHsxsp4jJHFLOtwVfQyPumU7vq8N-5iV8ADQc28jpFNwKUU-fUJkHonALYj8Vg'
        },
    };

    const res = http.get(url, params);
    check(res, {
        'is status 200': (r) => r.status === 200,
    });
}

export function handleSummary(data) {
  return {
    "reports/mostrar_productos.html": htmlReport(data),
  };
}