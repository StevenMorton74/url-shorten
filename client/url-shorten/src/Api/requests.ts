import Api from './constants';

const sendCreateRequest = async (
  url: string,
  setResult: (arg1: string) => void,
  setError: (arg2: string) => void,
) => {
  setResult('');
  setError('');

  const requestOptions = {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ Url: url }),
  };

  const response = await fetch(`${Api.Hostname}/create`, requestOptions);

  if (response.ok) {
    const result = await response.json();
    setResult(result.shortUrl);
  } else {
    const error = await response.text();
    setError(error);
  }
};

export default sendCreateRequest;
