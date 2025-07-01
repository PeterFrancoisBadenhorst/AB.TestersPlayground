import React, { useState, useEffect } from 'react';
import './App.css';

function App() {
  const [apiData, setApiData] = useState(null);
  const [loading, setLoading] = useState(false);

  const fetchData = async () => {
    setLoading(true);
    try {
      // Try to fetch from API through nginx proxy
      const response = await fetch('/api/test');
      if (response.ok) {
        const data = await response.text();
        setApiData(data);
      } else {
        setApiData('API not available');
      }
    } catch (error) {
      setApiData('Error connecting to API');
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        <h1>Testers Playground - React Frontend</h1>
      </header>
      <main className="App-main">
        <div className="content">
          <h2>API Connection Test</h2>
          <button onClick={fetchData} disabled={loading}>
            {loading ? 'Loading...' : 'Test API Connection'}
          </button>
          <div className="api-result">
            <strong>API Response:</strong>
            <pre>{apiData || 'No data yet'}</pre>
          </div>
          
          <div className="info-section">
            <h3>Available Services</h3>
            <ul>
              <li>React Frontend (this app) - Port 3000</li>
              <li>Blazor Frontend - Port 5003</li>
              <li>API 1 - Port 5001</li>
              <li>API 2 - Port 5002</li>
              <li>Nginx Proxy - Port 80</li>
            </ul>
          </div>
        </div>
      </header>
    </div>
  );
}

export default App;
