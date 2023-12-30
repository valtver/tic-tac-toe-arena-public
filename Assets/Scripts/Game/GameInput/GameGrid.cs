using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundWin {
    public SymbolVar sym = new SymbolVar();
}

public class WinLine {
    public List<GameGridInput> unit = new List<GameGridInput>();
    public int score;
    public bool busy = true;
    public bool pointless = false;
}

public class GameGrid : _baseCompositionModule
{
    [Header("Behavior")]
    public GameData data;
    public int gridSize;
    public int winLength;
    private List<WinLine> winLine = new List<WinLine>();
    public RoundWin roundWin = new RoundWin();

    [Header("Events")]
    public baseEvent eventWin;
    public baseEvent eventDraw;
    public baseEvent eventGameGridReady;
    public baseEvent eventGridInitComplete;

    [Header("AI")]
    public float AITurnDelay;
    [Range(0.0f, 1.0f)]
    public float AIAssholeChance = 1f;

    // [SerializeField]
    private Dictionary<string, GameGridInput> units = new Dictionary<string, GameGridInput>();

    public override IEnumerator Init() {
        yield return StartCoroutine(InitUnits());
        yield return StartCoroutine(GenerateWinLines());
        yield return StartCoroutine(GenerateGridScoreOrWinOrReady(false));
        Debug.Log("Grid Init complete");
        yield return null;
    }

    public IEnumerator InitUnits() {
        var childrenUnits = GetComponentsInChildren<GameGridInput>();
        for(int i = 0; i < childrenUnits.Length; i++) {
            units.Add( childrenUnits[i].id.x.ToString() + childrenUnits[i].id.y.ToString(), childrenUnits[i] );
            yield return StartCoroutine(childrenUnits[i].Init());
        }
        Debug.Log("Units init complete. " + units.Count + " units added. Unit test: Unit 12 = " + units["12"].id.x.ToString() + units["12"].id.y.ToString());
        yield return null;
    }

    public void AiTurn() {
        StartCoroutine(AIMove());
    }

    public bool IsAsshole() {
        float chance = Random.Range(0f, 1f);
        if(chance <= AIAssholeChance) {
            return true;
        }
        return false;
    }
    
    public IEnumerator AIMove() {
        yield return new WaitForSecondsRealtime(AITurnDelay);
        List<WinLine> winLines = new List<WinLine>();
        List<WinLine> assholeLines = new List<WinLine>();
        List<WinLine> warriorLines = new List<WinLine>();
        List<WinLine> whateverLines = new List<WinLine>();
        List<GameGridInput> choices = new List<GameGridInput>();

        bool win, asshole, warrior, whatever;
        win = asshole = warrior = whatever = false;

        int PCmark = (int)data.ActivePlayer.symbol;

        Debug.Log("PC symbol is: " + PCmark);
        //CHECK TO WIN OR DROP SOME SHIT ON A FAN
        for(int i = 0; i < winLine.Count; i++) {
            if(!winLine[i].busy && !winLine[i].pointless) {
                if( winLine[i].score == (PCmark * (winLength-1)) ) {
                    win = true;
                    winLines.Add(winLine[i]);
                }
                if( (winLine[i].score == (-PCmark * (winLength-1))) || (winLine[i].score == (-PCmark * (winLength-2))) ) {
                    asshole = IsAsshole();
                    assholeLines.Add(winLine[i]);
                    // winLine[i].pointless = true;
                }
            }
            
        }
        yield return null;
        //IF NONE OF THOSE -> CONTINUE YOUR TRY TO WIN
        
        if(!win && !asshole) {
            for(int i = (winLength-2); i > -1; i--) { //smaller score
                for(int j = 0; j < winLine.Count; j++) {                
                    if(!winLine[j].busy && !winLine[j].pointless) {
                        if((winLine[j].score == (PCmark * i)) && (i > 0)) {
                            warrior = true;
                            warriorLines.Add(winLine[j]);
                            break;
                        }
                        else {
                            whateverLines.Add(winLine[j]);
                        }
                    }
                    if(!winLine[j].busy && winLine[j].pointless) {
                        whateverLines.Add(winLine[j]);
                    }
                }
                if(warrior) {
                    break;
                }
            }
            if(!warrior) {
                whatever = true;
            }
        }

        // if(!win || !asshole) {
        //         for(int i = 0; i < winLine.Count; i++) {                
        //             if(!winLine[i].busy) {
        //                 int potentialScore = 0;
        //                 for(int n = 0; n < winLine[i].unit.Count; n++) {
        //                     if(winLine[i].unit[n].sym == 0) {
        //                         potentialScore++;

        //                     }
        //                 }
        //                 if(winLine[i].score + (PCmark * potentialScore) == PCmark * winLength) {
        //                     warrior = true;
        //                     warriorLines.Add(winLine[i]);
        //                 }
        //                 else {
        //                     whateverLines.Add(winLine[i]);
        //                 }
        //             }   
        //         }
        //         if(!warrior) {
        //             whatever = true;
        //         }
            
        // }
        yield return null;
        Debug.Log("PC: Win = " + win + ", Asshole = " + asshole + " Whatever = " + whatever + " Warrior = " + warrior);
        if(win) {
            Debug.Log("PC is Going to win!");
            for(int i = 0; i < winLines.Count; i++) {
                for(int n = 0; n < winLines[i].unit.Count; n++) {
                    if(winLines[i].unit[n].sym == 0) {
                        choices.Add(winLines[i].unit[n]);
                    }
                }
                // yield return null;
            }
            int rand = Random.Range(0, choices.Count);
            choices[rand].gameObject.GetComponent<UserInputComponent>().ClickUp();

        }
        else if(asshole) {
            Debug.Log("PC is Going asshole!");
            for(int i = 0; i < assholeLines.Count; i++) {
                for(int n = 0; n < assholeLines[i].unit.Count; n++) {
                    if(assholeLines[i].unit[n].sym == 0) {
                        choices.Add(assholeLines[i].unit[n]);

                    }
                }
                // yield return null;
            }
            int rand = Random.Range(0, choices.Count);
            choices[rand].gameObject.GetComponent<UserInputComponent>().ClickUp();
        }
        else if(warrior) {
            for(int i = 0; i < warriorLines.Count; i++) {
                for(int n = 0; n < warriorLines[i].unit.Count; n++) {
                    if(warriorLines[i].unit[n].sym == 0) {
                        choices.Add(warriorLines[i].unit[n]);
                    }
                }
                // yield return null;
            } 
            int rand = Random.Range(0, choices.Count);
            choices[rand].gameObject.GetComponent<UserInputComponent>().ClickUp(); 
        }
        else if(whatever) {
            for(int i = 0; i < whateverLines.Count; i++) {
                for(int n = 0; n < whateverLines[i].unit.Count; n++) {
                    if(whateverLines[i].unit[n].sym == 0) {
                        choices.Add(whateverLines[i].unit[n]);
                    }
                }
                // yield return null;
            }
            int rand = Random.Range(0, choices.Count);
            choices[rand].gameObject.GetComponent<UserInputComponent>().ClickUp(); 
        }
    }

    public IEnumerator GenerateGridScoreOrWinOrReady(bool play = true) {
        // Debug.Log("Grid Score generate...");
        bool win = false;
        bool draw = true;
        for(int i = 0; i < winLine.Count; i++) {
            int scr = 0;
            int freeSym = 0;
            winLine[i].busy = true;
            winLine[i].pointless = false;
            for(int n = 0; n < winLine[i].unit.Count; n++) {
                if((int)winLine[i].unit[n].sym == 0) {
                    draw = false;
                    winLine[i].busy = false;
                    freeSym++;
                }
                scr += (int)winLine[i].unit[n].sym;
            }
            // Debug.Log("WinLine " + i + " busy: " + winLine[i].busy);
            winLine[i].score = scr;
            if( Mathf.Abs(winLine[i].score) == winLength ) {
                roundWin.sym = winLine[i].unit[0].sym;
                for(int w = 0; w < winLine[i].unit.Count; w++) {
                    winLine[i].unit[w].FlagWin();
                }
                win = true;
                break;
            }
            if(freeSym <= winLength-1 && scr >= -1 && scr <= 1) {
                winLine[i].pointless = true;
            }
            // yield return null;
        }
        yield return null;

        if(win) {
            Data.Instance.Game.LastRoundWinner = roundWin.sym;
            yield return new WaitForSeconds(0.25f);
            eventWin.Raise(this.gameObject);
            roundWin = new RoundWin();
        }
        else if(draw) {
            Data.Instance.Game.LastRoundWinner = roundWin.sym;
            yield return new WaitForSeconds(0.25f);
            eventDraw.Raise(this.gameObject);
        }
        else if(play){
            eventGameGridReady.Raise(this.gameObject);
        }
        else {
            eventGridInitComplete.Raise(this.gameObject);
        }
    }
    
    private IEnumerator GenerateWinLines() {
        // Debug.Log("Win lines generate:");
        WinLine winL;
        int fitSteps = gridSize - winLength;

        for(int yPosOffset = 0; yPosOffset <= fitSteps; yPosOffset++) {

            for(int xPosOffset = 0; xPosOffset <= fitSteps; xPosOffset++) {

                // Debug.Log(xPosOffset + "" + yPosOffset + " square wins:");
                // Debug.Log("Horizontals");
                for(int y = yPosOffset; y < (winLength + yPosOffset); y++) { //HORISONTALS
                    winL = new WinLine();
                    // Debug.Log("Line " + winLine.Count + " ----------");
                    for(int x = xPosOffset; x < (winLength + xPosOffset); x++) {
                        winL.unit.Add(units[x.ToString() + y.ToString()]);
                        // Debug.Log(winL.unit[winL.unit.Count-1].id.x + " " + winL.unit[winL.unit.Count-1].id.y);
                    }
                    winLine.Add(winL);
                }
                // yield return null;
                // Debug.Log("Verticals");
                for(int x = xPosOffset; x < (winLength + xPosOffset); x++) { //VERTICALS
                    winL = new WinLine();
                    // Debug.Log("Line " + winLine.Count + " ----------");
                    for(int y = yPosOffset; y < (winLength + yPosOffset); y++) {
                        winL.unit.Add(units[x.ToString() + y.ToString()]);
                        // Debug.Log(winL.unit[winL.unit.Count-1].id.x + " " + winL.unit[winL.unit.Count-1].id.y);
                    }
                    winLine.Add(winL);
                }
                // yield return null;
                // Debug.Log("Diagonals");
                int dy = yPosOffset;
                winL = new WinLine();
                // Debug.Log("Line " + winLine.Count + " ----------");
                for(int x = xPosOffset; x < (winLength + xPosOffset); x++) { //DIAGONALS
                    winL.unit.Add(units[x.ToString() + dy.ToString()]);
                    // Debug.Log(winL.unit[winL.unit.Count-1].id.x + " " + winL.unit[winL.unit.Count-1].id.y);
                    dy++;
                }
                winLine.Add(winL);
                // yield return null;

                dy = yPosOffset + (winLength - 1);
                winL = new WinLine();
                // Debug.Log("Line " + winLine.Count + " ----------");
                for(int x = xPosOffset; x < (winLength + xPosOffset); x++) { //DIAGONALS
                    winL.unit.Add(units[x.ToString() + dy.ToString()]);
                    // Debug.Log(winL.unit[winL.unit.Count-1].id.x + " " + winL.unit[winL.unit.Count-1].id.y);
                    dy--;
                }
                winLine.Add(winL);
                // Debug.Log("Generated " + winLine.Count + " winlines!");
                yield return null;
            }

        }

    }

    public void GridStateCheck() {
        StartCoroutine(GenerateGridScoreOrWinOrReady());
    }

    public void GridInputClick(GameObject gameGridInput) {
        StopAllCoroutines();
        eventGameGridReady.Raise(this.gameObject);
    }

    public void GridRoundOutro() {
        //play winner/hide other
        GameGridInput lastUnit;

        foreach(KeyValuePair<string, GameGridInput> unit in units) {
            if(unit.Value.isWinUnit) {
                AudioComponent.PlaySoundPassive("win");
                unit.Value.Highlight(true);
                lastUnit = unit.Value;
            }
            else {
                // AudioComponent.PlaySound("swing");
                unit.Value.Dim();
            }
        }
        // Invoke("Reset", 1.0f);
    }

    public void GridLevelOutro() {
        //play winner/hide other
        GameGridInput lastUnit;

        foreach(KeyValuePair<string, GameGridInput> unit in units) {
            if(unit.Value.isWinUnit) {
                AudioComponent.PlaySoundPassive("win");
                unit.Value.Highlight(false);
                lastUnit = unit.Value;
            }
            else {
                // AudioComponent.PlaySound("swing");
                unit.Value.Dim();
            }
        }
        // Invoke("Reset", 1.0f);
    }

    public override void Reset() {
        // base.Reset();
        ResetUnits();
        StopAllCoroutines();
        StartCoroutine(GenerateGridScoreOrWinOrReady());
    }

    private void ResetUnits() {
        foreach(KeyValuePair<string, GameGridInput> unit in units) {
            unit.Value.Reset();
        }
    }

}
