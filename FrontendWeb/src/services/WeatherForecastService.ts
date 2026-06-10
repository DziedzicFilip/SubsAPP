import type { WeatherData } from '../Models/WeatherForcastModel';

export const getWeatherForecasts = async (): Promise<WeatherData[]> => {
	try {
		const response = await fetch('/api/weatherforecast');
		if (!response.ok) {
			throw new Error(`HTTP error! status: ${response.status}`);
		}
		const data = await response.json();
		return data;
	} catch (err) {
		throw err instanceof Error ? err : new Error('Bad response from server');
	}
};
