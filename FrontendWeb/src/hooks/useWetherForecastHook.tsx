import { useEffect, useState } from 'react';
import { getWeatherForecasts } from '../services/WeatherForecastService';
import type { WeatherData } from '../Models/WeatherForcastModel';

export const useWeatherForecastHook = () => {
	const [forecasts, setForecasts] = useState<WeatherData[]>([]);
	const [error, setError] = useState<Error | null>(null);
	const [loading, setLoading] = useState(true);

	useEffect(() => {
		getWeatherForecasts()
			.then((data) => setForecasts(data))
			.catch((err) =>
				setError(
					err instanceof Error
						? err
						: new Error('Nie można pobrać danych pogodowych'),
				),
			)
			.finally(() => setLoading(false));
	}, []);

	return { forecasts, error, loading };
};
