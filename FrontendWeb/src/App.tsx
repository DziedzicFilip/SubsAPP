import { useEffect, useState } from 'react';

import './App.css';

interface WeatherData {
	date: string;
	temperatureC: number;
	temperatureF: number;
	summary: string;
}

function App() {
	const [forecasts, setForecasts] = useState<WeatherData[]>([]);
	const [error, setError] = useState<string | null>(null);
	const [loading, setLoading] = useState<boolean>(true);

	useEffect(() => {
		const fetchData = async () => {
			try {
				const response = await fetch('api/weatherforecast');
				if (!response.ok) {
					throw new Error(`HTTP error! status: ${response.status}`);
				}
				const data = await response.json();
				setForecasts(data);
			} catch (err) {
				setError(err instanceof Error ? err.message : 'Unknown error');
			} finally {
				setLoading(false);
			}
		};
		fetchData();
	}, []);

	// fetch('http://localhost:5000/api/weatherforecast')
	// 	.then((response) => response.json())
	// 	.then((data) => setForecasts(data));

	return (
		<div className='container'>
			<h1>SubsAPP - Połączenie z Backendem .NET</h1>

			<div className='card'>
				{loading && <p>Ładowanie danych...</p>}
				{error && <p style={{ color: 'red' }}>Błąd: {error}</p>}
				{!error && !loading && (
					<p>
						Poniżej znajduje się tabela z danymi pogodowymi pobranymi z
						backendowego API .NET. Każdy wiersz reprezentuje prognozę pogody na
						dany dzień, zawierając datę, temperaturę w stopniach Celsjusza oraz
						krótkie podsumowanie warunków pogodowych.
					</p>
				)}
				<table
					border={1}
					style={{ width: '100%', marginTop: '10px', textAlign: 'left' }}
				>
					<thead>
						<tr>
							<th>Data</th>
							<th>Temp (°C)</th>
							<th>Podsumowanie</th>
						</tr>
					</thead>
					<tbody>
						{forecasts.map((f, index) => (
							<tr key={index}>
								<td>{new Date(f.date).toLocaleDateString()}</td>
								<td>{f.temperatureC}°C</td>
								<td>{f.summary}</td>
							</tr>
						))}
					</tbody>
				</table>
			</div>
		</div>
	);
}

export default App;
