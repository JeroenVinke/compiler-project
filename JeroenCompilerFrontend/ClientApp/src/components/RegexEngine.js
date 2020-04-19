import React, { Component } from 'react';
import LoadingOverlay from 'react-loading-overlay';

export class RegexEngine extends Component {
    static displayName = RegexEngine.name;

    constructor(props) {
        super(props);
        this.state = { regex: '([0-9])*#', input: '135', loading: true, matches: false };
        this.regexChanged = this.regexChanged.bind(this);
        this.inputChanged = this.inputChanged.bind(this);
        this.reload = this.reload.bind(this);
    }

    componentDidMount() {
      this.internalRefresh();
    }

    regexChanged(e) {
        this.setState({ regex: e.target.value });

        this.reload();
    }

    inputChanged(e) {
        this.setState({ input: e.target.value });

        this.reload();
    }

    reload() {
      if (!this.state.loading) {
          this.setState({ loading: true });
          
          this.refs.graphContainer1.innerHTML = '';
          this.refs.graphContainer2.innerHTML = '';

          this.internalRefresh();
        }
    }

    internalRefresh() {
      this.getResult()
      .then(() => {
        console.log('DONE')
        this.setState({ loading: false });
      })
      .catch(() => {
        this.setState({ loading: false });
      });
    }

    getResult() {
        return fetch("https://localhost:44353/api/compiler/regex", {
            body: JSON.stringify({
                regex: this.state.regex,
                input: this.state.input
            }),
            headers: {
                'Content-Type': 'application/json'
            },
            method: "post"
        })
            .then(res => res.json())
            .then(res => {
                this.setState({
                    syntaxtree: res.syntaxTree,
                    dfa: res.dfa,
                    matches: res.matches
                });

                let viz = new window.Viz({ workerURL: 'full.render.js' });

                return Promise.all([
                  viz.renderSVGElement(res.syntaxTree)
                  .then(element => {
                      this.refs.graphContainer1.append(element);
                  })
                  .catch(error => {
                      viz = new window.Viz({ workerURL: 'full.render.js' });
                      console.error(error);
                  }),

                  viz.renderSVGElement(res.dfa)
                  .then(element => {
                      this.refs.graphContainer2.append(element);
                  })
                  .catch(error => {
                      viz = new window.Viz({ workerURL: 'full.render.js' });
                      console.error(error);
                  })
                ])
            });
    }

    render() {
        return (
            <div className="regex">
                <h1>Regex engine</h1>

                <div className="row">
                    <div className="left">
                        <p>Examples:</p>
                        <ul>
                          <li>(foo|bar)#</li>
                          <li>([0-9])+#</li>
                        </ul>

                        <p>Regex (followed by #):</p>
                        <textarea value={this.state.regex} onChange={this.regexChanged}></textarea>
                        <p>Input:</p>
                        <textarea value={this.state.input} onChange={this.inputChanged}></textarea>

                        {
                          !this.state.loading && <p>Match? {this.state.matches ? 'Yes' : 'No' }</p>
                        }

                        <button onClick={this.reload}>Update</button>
                    </div>

                    <div className="center">
                      <LoadingOverlay
                          active={this.state.loading}
                          spinner
                          text='Loading.....'
                          >
                            { this.state.loading && <p>Loading</p>}
                            <div ref="graphContainer1" className="graphContainer1">
                            </div>
                      </LoadingOverlay>
                    </div>

                    <div className="right">
                      <LoadingOverlay
                          active={this.state.loading}
                          spinner
                          text='Loading.....'
                          >
                            { this.state.loading && <p>Loading</p>}
                          <div ref="graphContainer2" className="graphContainer2">
                          </div>
                      </LoadingOverlay>
                    </div>
                </div>
            </div>
        );
    }
}
