import { Col, Row } from 'antd';
import React, { useCallback, useEffect, useState } from 'react';
import sendCreateRequest from '../Api/requests';
import './App.css';
import ErrorMessage from './ErrorMessage';
import InputUrl from './InputUrl';
import ShortUrl from './ShortUrl';

function App() {
  const [error, setError] = useState('');
  const [url, setUrl] = useState('');
  const [result, setResult] = useState('');
  const handleChange = (e:React.ChangeEvent<HTMLInputElement>) => setUrl(e.target.value);

  const handleSubmit = useCallback(async () => {
    await sendCreateRequest(url, setResult, setError);
  }, [url]);

  return (
    <div className="App">
      <Row>
        <Col span={24}>
          {error && <ErrorMessage error={error} />}
        </Col>
      </Row>
      <Row>
        <Col span={24}>
          <h1>URL Shorten</h1>
          <InputUrl handleChange={handleChange} handleSubmit={handleSubmit} />
        </Col>
      </Row>
      <Row>
        <Col span={24}>
          {result && <ShortUrl shortUrl={result} />}
        </Col>
      </Row>
    </div>
  );
}

export default App;
