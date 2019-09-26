
classdef DotNetBenchmark < handle
    properties
        cnt
    end
    methods
        function obj = DotNetBenchmark()
            %             profile('on','-detail','builtin','-history')
            
            %% load Net class
            asmFile = fullfile(pwd,"./dotNetClass\bin\Release\dotNetClass.dll");
            asm = NET.addAssembly(asmFile);
            
            class1 = myNamespace.dotNetClass();
            
            %% polling test #1
            obj.cnt = 0;
            t1 = tic;
            while(toc(t1) < 10)
                e = class1.GetSingleDataset();
                obj.extractAndDoSomethingWithData(e);
            end
            
            
            thousandElementsPerSec = (obj.cnt / (toc(t1)*1000)); % *1000 because toc is in sec
            fprintf("polling speed (class1.GetSingleDataset(): " + thousandElementsPerSec + " kPcs/s\n");
            
            
            %% polling test #2
            obj.cnt = 0;
            t1 = tic;
            while(toc(t1) < 10)
                e = GetSingleDataset(class1);
                obj.extractAndDoSomethingWithData(e);
            end
            
            thousandElementsPerSec = (obj.cnt / (toc(t1)*1000)); % *1000 because toc is in sec
            fprintf("polling speed (GetSingleDataset(class1): " + thousandElementsPerSec + " kPcs/s\n");
            
            
            %% event test
            class2 = myNamespace.dotNetClass();
                                    
            % attach listener, why is the anonymous handle faster?
            %           lis = addlistener(evnt, 'DataIsReady', @(src,evnt)DataIsReady(obj,src,evnt));
            %           lis = addlistener(netClass, 'DataIsReady', @obj.DataIsReady);
            lis = addlistener(class2, 'DataIsReady', @(src,evnt)obj.DataIsReady(src,evnt)); % faster than the rest!
            
            
            obj.cnt = 0;
            t2 = tic;
            class2.StartDataGenerationEvent();
            
            thousandElementsPerSec = (obj.cnt / (toc(t2)*1000)); % *1000 because toc is in sec
            fprintf("event speed: " + thousandElementsPerSec + " kPcs/s\n");
            
            delete(obj)
        end
        
        function extractAndDoSomethingWithData(obj,e)
            obj.cnt = obj.cnt + 1;
            
            % 2 split process is faster than direct cast
            time = e.Time; % handle
            data = e.Data;
            idx = e.Idx;
            
            Time = double(time); % value
            Data = double(data);
            idx = uint32(idx);
        end
        
        function DataIsReady(obj, ~, evnt)
            obj.extractAndDoSomethingWithData(evnt);
        end
    end
end
